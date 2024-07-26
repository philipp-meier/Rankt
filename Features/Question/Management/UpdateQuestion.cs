using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.Question.Management;

internal static class UpdateQuestionEndpoint
{
    internal static void MapUpdateQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("questions/{id:guid}", async (Guid id, UpdateQuestionRequest request,
            ApplicationDbContext dbContext,
            ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<IdentityUser>>();
            var user = await userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var question = await dbContext.Questions
                .Where(x => x.CreatedBy == user)
                .Include(x => x.Status)
                .Include(question => question.Type)
                .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

            if (question == null)
            {
                return Results.NotFound();
            }

            if (question.Status.Id != QuestionStatus.New.Id)
            {
                return Results.BadRequest(new
                {
                    error = "Question was already published and cannot be changed anymore."
                });
            }

            question.Title = request.Title;

            var existingOptionIdentifiers = question.Options
                .Select(x => x.ExternalIdentifier)
                .ToArray();

            var modifiedOptionsIdentifiers = request.Options
                .Where(x => x.Identifier != null)
                .Select(x => x.Identifier)
                .OfType<Guid>()
                .ToArray();

            var toRemoveIdentifiers = existingOptionIdentifiers.Except(modifiedOptionsIdentifiers);
            foreach (var toRemove in toRemoveIdentifiers)
            {
                var toRemoveOption = question.Options.Single(x => x.ExternalIdentifier == toRemove);
                question.Options.Remove(toRemoveOption);
            }

            var toUpdateIdentifiers = modifiedOptionsIdentifiers.Intersect(existingOptionIdentifiers);
            foreach (var toUpdate in toUpdateIdentifiers)
            {
                var source = request.Options.Single(x => x.Identifier == toUpdate);
                var target = question.Options.Single(x => x.ExternalIdentifier == toUpdate);

                target.Title = source.Title;
                target.Description = source.Description;
            }

            foreach (var toAdd in request.Options.Where(x => x.Identifier == null))
            {
                question.Options.Add(new QuestionOption
                {
                    Title = toAdd.Title, Position = toAdd.Position, Description = toAdd.Description
                });
            }

            if (question.Type.Identifier != QuestionType.Voting.Identifier)
            {
                question.MaxSelectableItems = request.MaxSelectableItems.HasValue
                    ? Math.Min(question.Options.Count, request.MaxSelectableItems.Value)
                    : request.MaxSelectableItems;
            }

            dbContext.Update(question);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        });
    }

    private record UpdateQuestionRequest(
        string Title,
        int? MaxSelectableItems,
        List<UpdateQuestionOptionRequest> Options);

    private record UpdateQuestionOptionRequest(Guid? Identifier, string Title, int Position, string? Description);
}
