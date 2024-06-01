using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion.Execution;

internal static class GetRankingQuestionEndpoint
{
    internal static void MapGetRankingQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapMethods("questions/{id:guid}", ["HEAD"], async (Guid id,
            ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal, UserManager<IdentityUser> userManager,
            CancellationToken cancellationToken) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);

            var rankingQuestion = await dbContext.RankingQuestions
                .Where(x => x.CreatedBy == user || (x.Status.Id != RankingQuestionStatus.New.Id &&
                                                    x.Status.Id != RankingQuestionStatus.Archived.Id))
                .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

            return rankingQuestion != null ? Results.Ok() : Results.NotFound();
        }).RequireRateLimiting("fixed").AllowAnonymous();

        app.MapGet("questions/{id:guid}", async (Guid id, ApplicationDbContext dbContext,
            ClaimsPrincipal claimsPrincipal, UserManager<IdentityUser> userManager,
            CancellationToken cancellationToken) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);

            var rankingQuestion = await dbContext.RankingQuestions
                .Where(x => x.CreatedBy == user || (x.Status.Id != RankingQuestionStatus.New.Id &&
                                                    x.Status.Id != RankingQuestionStatus.Archived.Id))
                .Include(rankingQuestion => rankingQuestion.Status)
                .Include(rankingQuestion => rankingQuestion.Responses)
                .FirstAsync(x => x.ExternalIdentifier == id, cancellationToken);

            return new GetRankingQuestionResponse
            {
                Identifier = id,
                Title = rankingQuestion.Title,
                Status = rankingQuestion.Status.Name,
                ResponseCount = rankingQuestion.Responses.Count,
                MaxSelectableItems = rankingQuestion.MaxSelectableItems,
                Created = rankingQuestion.Created,
                Options = rankingQuestion.Options.Select(x => new GetRankingQuestionOptionResponse
                {
                    Identifier = x.ExternalIdentifier, Title = x.Title, Description = x.Description
                }).ToList()
            };
        });
    }

    private record GetRankingQuestionResponse
    {
        public Guid Identifier { get; set; }
        public required string Title { get; set; }
        public required string Status { get; set; }
        public required int ResponseCount { get; set; }
        public required DateTime Created { get; set; }
        public int? MaxSelectableItems { get; set; }
        public IList<GetRankingQuestionOptionResponse> Options { get; set; } = [];
    }

    private record GetRankingQuestionOptionResponse
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Guid Identifier { get; set; }
    }
}
