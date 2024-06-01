using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion.Management;

internal static class CreateRankingQuestionEndpoint
{
    internal static void MapCreateRankingQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("questions", async (CreateRankingQuestionRequest command, ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var status = await dbContext.RankingQuestionStatus
                .FirstAsync(x => x.Id == RankingQuestionStatus.New.Id, cancellationToken);

            var question = new Entities.RankingQuestion
            {
                Title = command.Title, Status = status, Options = new List<RankingQuestionOption>()
            };

            foreach (var option in command.Options)
            {
                question.Options.Add(new RankingQuestionOption
                {
                    Title = option.Title, Description = option.Description
                });
            }

            question.MaxSelectableItems = command.MaxSelectableItems.HasValue
                ? Math.Min(question.Options.Count, command.MaxSelectableItems.Value)
                : command.MaxSelectableItems;

            dbContext.Add(question);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(new
            {
                identifier = question.ExternalIdentifier, status = question.Status.Name, created = question.Created
            });
        });
    }

    private record CreateRankingQuestionRequest(
        string Title,
        int? MaxSelectableItems,
        IList<CreateRankingQuestionOptionRequest> Options);

    private record CreateRankingQuestionOptionRequest(string Title, string? Description);
}
