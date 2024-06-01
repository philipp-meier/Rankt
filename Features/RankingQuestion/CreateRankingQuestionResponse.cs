using Carter;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion;

public record CreateRankingQuestionResponseRequest
{
    public required string Username { get; set; }
    public IList<CreateRankingQuestionResponseItemRequest> Options { get; set; } = [];
}

public record CreateRankingQuestionResponseItemRequest
{
    public required int Rank { get; set; }
    public required Guid Identifier { get; set; }
}

public class CreateRankingQuestionResponseModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/questions/{id:guid}/response",
            async (Guid id, CreateRankingQuestionResponseRequest request, ApplicationDbContext dbContext,
                CancellationToken cancellationToken) =>
            {
                var rankingQuestion = await dbContext.RankingQuestions
                    .Where(x => x.Status.Id == RankingQuestionStatus.Published.Id ||
                                x.Status.Id == RankingQuestionStatus.InProgress.Id)
                    .Include(rankingQuestion => rankingQuestion.Status)
                    .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

                if (rankingQuestion == null)
                {
                    return Results.NotFound();
                }

                var response = new RankingQuestionResponse
                {
                    Username = request.Username, RankingQuestion = rankingQuestion
                };

                var rank = 1;
                foreach (var optionRanking in request.Options.OrderBy(x => x.Rank))
                {
                    var option = await dbContext.RankingQuestionOptions
                        .FirstOrDefaultAsync(x => x.ExternalIdentifier == optionRanking.Identifier &&
                                                  x.RankingQuestion.ExternalIdentifier == id, cancellationToken);

                    // Invalid data is skipped
                    if (option == null)
                    {
                        continue;
                    }

                    response.ResponseItems.Add(new RankingQuestionResponseItem
                    {
                        Rank = rank++, RankingQuestionResponse = response, RankingQuestionOption = option
                    });
                }

                // Set the poll to "in progress", since votes are incoming.
                if (rankingQuestion.Status.Id != RankingQuestionStatus.InProgress.Id)
                {
                    rankingQuestion.Status = RankingQuestionStatus.InProgress;
                }

                dbContext.Add(response);

                await dbContext.SaveChangesAsync(cancellationToken);

                return Results.Ok(new { identifier = response.ExternalIdentifier });
            }).RequireRateLimiting("fixed").AllowAnonymous();
    }
}
