using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.Question.Management;

internal static class CreateQuestionEndpoint
{
    internal static void MapCreateQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("questions", async (CreateQuestionRequest command, ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var status = await dbContext.QuestionStatus
                .FirstAsync(x => x.Id == QuestionStatus.New.Id, cancellationToken);

            var type = await dbContext.QuestionType
                .FirstAsync(x => x.Identifier == command.Type, cancellationToken);

            var question = new Entities.Question
            {
                Title = command.Title, Status = status, Type = type, Options = new List<QuestionOption>()
            };

            foreach (var option in command.Options)
            {
                question.Options.Add(new QuestionOption
                {
                    Title = option.Title, Position = option.Position, Description = option.Description
                });
            }

            if (type.Identifier != QuestionType.Voting.Identifier)
            {
                question.MaxSelectableItems = command.MaxSelectableItems.HasValue
                    ? Math.Min(question.Options.Count, command.MaxSelectableItems.Value)
                    : command.MaxSelectableItems;
            }

            dbContext.Add(question);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(new
            {
                identifier = question.ExternalIdentifier, status = question.Status.Name, created = question.Created
            });
        });
    }

    private record CreateQuestionRequest(
        string Title,
        string Type,
        int? MaxSelectableItems,
        IList<CreateQuestionOptionRequest> Options);

    private record CreateQuestionOptionRequest(string Title, int Position, string? Description);
}
