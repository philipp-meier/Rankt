using System.Security.Claims;
using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion;

public record UpdateRankingQuestionStatusRequest
{
    public required string Identifier { get; set; }
}

public class UpdateRankingQuestionStatusModule : ICarterModule
{
    private static readonly (string current, string target)[] s_allowedStatusTransitions =
    [
        (RankingQuestionStatus.New.Identifier, RankingQuestionStatus.Published.Identifier),
        (RankingQuestionStatus.Published.Identifier, RankingQuestionStatus.InProgress.Identifier),
        (RankingQuestionStatus.Published.Identifier, RankingQuestionStatus.Completed.Identifier),
        (RankingQuestionStatus.InProgress.Identifier, RankingQuestionStatus.Completed.Identifier),
        (RankingQuestionStatus.Completed.Identifier, RankingQuestionStatus.Archived.Identifier)
    ];

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/questions/{id:guid}/status", async (Guid id, UpdateRankingQuestionStatusRequest request,
            ApplicationDbContext dbContext,
            ClaimsPrincipal claimsPrincipal, UserManager<IdentityUser> userManager,
            CancellationToken cancellationToken) =>
        {
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

            var newStatus =
                await dbContext.RankingQuestionStatus.FirstOrDefaultAsync(x => x.Identifier == request.Identifier,
                    cancellationToken);

            if (newStatus == null)
            {
                return Results.BadRequest(new { error = $"Invalid status identifier \"{request.Identifier}\"." });
            }

            var currentStatus = rankingQuestion.Status;

            // TODO: Improve "StateMachine"
            if (!s_allowedStatusTransitions.Any(x =>
                    x.current == currentStatus.Identifier && x.target == newStatus.Identifier))
            {
                return Results.BadRequest(new
                {
                    error = $"A status change from \"{currentStatus.Name}\" to \"{newStatus.Name}\" is not allowed."
                });
            }

            rankingQuestion.Status = newStatus;

            dbContext.Update(rankingQuestion);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        }).RequireAuthorization();
    }
}
