using Carter;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion;

public record CreateRankingQuestionRequest
{
    public required string Title { get; set; }
    public int? MaxSelectableItems { get; set; }

    public IList<CreateRankingQuestionOptionRequest> Options { get; set; } = [];
}

public record CreateRankingQuestionOptionRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}

public class CreateRankingQuestionModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/questions", async (CreateRankingQuestionRequest command, ApplicationDbContext dbContext,
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
        }).RequireAuthorization();
    }
}
