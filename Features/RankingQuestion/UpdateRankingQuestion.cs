using System.Security.Claims;
using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion;

public record UpdateRankingQuestionRequest
{
    public required string Title { get; set; }
    public List<UpdateRankingQuestionOptionRequest> Options { get; set; } = [];
}

public record UpdateRankingQuestionOptionRequest
{
    public Guid? Identifier { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}

public class UpdateRankingQuestion : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/questions/{id:guid}", async (Guid id, UpdateRankingQuestionRequest request,
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

            dbContext.Update(rankingQuestion);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        }).RequireAuthorization();
    }
}
