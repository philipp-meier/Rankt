using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion.Management;

internal static class UpdateRankingQuestionEndpoint
{
    internal static void MapUpdateRankingQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("questions/{id:guid}", async (Guid id, UpdateRankingQuestionRequest request,
            ApplicationDbContext dbContext,
            ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<IdentityUser>>();
            var user = await userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var rankingQuestion = await dbContext.RankingQuestions
                .Where(x => x.CreatedBy == user)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

            if (rankingQuestion == null)
            {
                return Results.NotFound();
            }

            if (rankingQuestion.Status.Id != RankingQuestionStatus.New.Id)
            {
                return Results.BadRequest(new
                {
                    error = "Question was already published and cannot be changed anymore."
                });
            }

            rankingQuestion.Title = request.Title;

            var existingOptionIdentifiers = rankingQuestion.Options
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
                var toRemoveOption = rankingQuestion.Options.Single(x => x.ExternalIdentifier == toRemove);
                rankingQuestion.Options.Remove(toRemoveOption);
            }

            var toUpdateIdentifiers = modifiedOptionsIdentifiers.Intersect(existingOptionIdentifiers);
            foreach (var toUpdate in toUpdateIdentifiers)
            {
                var source = request.Options.Single(x => x.Identifier == toUpdate);
                var target = rankingQuestion.Options.Single(x => x.ExternalIdentifier == toUpdate);

                target.Title = source.Title;
                target.Description = source.Description;
            }

            foreach (var toAdd in request.Options.Where(x => x.Identifier == null))
            {
                rankingQuestion.Options.Add(new RankingQuestionOption
                {
                    Title = toAdd.Title, Description = toAdd.Description
                });
            }

            rankingQuestion.MaxSelectableItems = request.MaxSelectableItems.HasValue
                ? Math.Min(rankingQuestion.Options.Count, request.MaxSelectableItems.Value)
                : request.MaxSelectableItems;

            dbContext.Update(rankingQuestion);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        });
    }

    private record UpdateRankingQuestionRequest(
        string Title,
        int? MaxSelectableItems,
        List<UpdateRankingQuestionOptionRequest> Options);

    private record UpdateRankingQuestionOptionRequest(Guid? Identifier, string Title, string? Description);
}
