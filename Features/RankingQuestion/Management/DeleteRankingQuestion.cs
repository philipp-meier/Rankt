using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion.Management;

internal static class DeleteRankingQuestionEndpoint
{
    internal static void MapDeleteRankingQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("questions/{id:guid}", async (Guid id, ApplicationDbContext dbContext,
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
                .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

            if (rankingQuestion == null)
            {
                return Results.NotFound();
            }

            dbContext.RankingQuestions.Remove(rankingQuestion);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        });
    }
}
