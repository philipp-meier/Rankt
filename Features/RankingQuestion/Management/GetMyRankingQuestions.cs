using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion.Management;

internal static class GetMyRankingQuestionEndpoint
{
    internal static void MapGetMyRankingQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("questions", async (ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal,
            UserManager<IdentityUser> userManager, CancellationToken cancellationToken) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var rankingQuestions = await dbContext.RankingQuestions
                .Where(x => x.CreatedBy == user)
                .Include(x => x.Status)
                .Include(x => x.Responses)
                .OrderBy(x => x.Title)
                .ToListAsync(cancellationToken);

            return new GetMyRankingQuestionResponse
            {
                RankingQuestions = rankingQuestions.Select(x => new RankingQuestionSummary
                {
                    Identifier = x.ExternalIdentifier,
                    Title = x.Title,
                    Created = x.Created,
                    Status = x.Status.Name,
                    ResponseCount = x.Responses.Count
                }).ToList(),
                AvailableStatusOptions = dbContext.RankingQuestionStatus.Select(x =>
                    new RankingQuestionStatusOption { Identifier = x.Identifier, Name = x.Name }).ToList()
            };
        });
    }

    private record GetMyRankingQuestionResponse
    {
        public required IList<RankingQuestionSummary> RankingQuestions { get; set; }
        public required IList<RankingQuestionStatusOption> AvailableStatusOptions { get; set; }
    }

    private record RankingQuestionSummary
    {
        public Guid Identifier { get; set; }
        public required string Title { get; set; }
        public required string Status { get; set; }
        public int ResponseCount { get; set; }
        public DateTime Created { get; set; }
    }

    private record RankingQuestionStatusOption
    {
        public required string Identifier { get; set; }
        public required string Name { get; set; }
    }
}
