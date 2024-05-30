using System.Security.Claims;
using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion;

public class GetRankingQuestionResultModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // TODO: Rate Limiting
        app.MapMethods("api/questions/{id:guid}/result", ["HEAD"],
            async (Guid id, ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal,
                UserManager<IdentityUser> userManager, CancellationToken cancellationToken) =>
            {
                var user = await userManager.GetUserAsync(claimsPrincipal);

                var rankingQuestion = await dbContext.RankingQuestions
                    .Where(x => x.Status.Id == RankingQuestionStatus.Completed.Id ||
                                (x.CreatedBy == user && x.Status.Id == RankingQuestionStatus.Archived.Id))
                    .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

                return rankingQuestion != null ? Results.Ok() : Results.NotFound();
            }).AllowAnonymous();

        app.MapGet("api/questions/{id:guid}/result",
            async (Guid id, ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal,
                UserManager<IdentityUser> userManager, CancellationToken cancellationToken) =>
            {
                var user = await userManager.GetUserAsync(claimsPrincipal);

                var rankingQuestion = await dbContext.RankingQuestions
                    .Where(x => x.Status.Id == RankingQuestionStatus.Completed.Id ||
                                (x.CreatedBy == user && x.Status.Id == RankingQuestionStatus.Archived.Id))
                    .Include(x => x.Responses)
                    .ThenInclude(x => x.ResponseItems)
                    .ThenInclude(x => x.RankingQuestionOption)
                    .FirstAsync(x => x.ExternalIdentifier == id, cancellationToken);

                // If there is a limit, only the first N items of each response are relevant
                // TODO: Improve UI/UX for that case
                var maxItems = rankingQuestion.MaxSelectableItems ?? rankingQuestion.Options.Count;
                var responses = rankingQuestion.Responses.ToArray();

                var result = new Dictionary<Guid, int>(); // Identifier, Points
                foreach (var response in responses)
                {
                    var items = response.ResponseItems
                        .OrderBy(x => x.Rank)
                        .Select(x => x.RankingQuestionOption)
                        .Take(maxItems);

                    var points = maxItems;
                    foreach (var item in items)
                    {
                        if (result.TryGetValue(item.ExternalIdentifier, out var count))
                        {
                            result[item.ExternalIdentifier] = count + points;
                        }
                        else
                        {
                            result.Add(item.ExternalIdentifier, points);
                        }

                        points--;
                    }
                }

                return new GetRankingQuestionResultResponse
                {
                    ResponseCount = rankingQuestion.Responses.Count,
                    Items = result.OrderByDescending(x => x.Value).Select(kv =>
                    {
                        var option = rankingQuestion.Options.First(x => x.ExternalIdentifier == kv.Key);
                        return new GetRankingQuestionResultItemResponse
                        {
                            Title = option.Title, Description = option.Description, Score = kv.Value
                        };
                    }).ToList()
                };
            }).AllowAnonymous();
    }
}

public record GetRankingQuestionResultResponse
{
    public IList<GetRankingQuestionResultItemResponse> Items { get; set; } = [];
    public required int ResponseCount { get; set; }
}

public record GetRankingQuestionResultItemResponse
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int Score { get; set; }
}
