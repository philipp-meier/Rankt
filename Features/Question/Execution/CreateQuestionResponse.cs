using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.Question.Execution;

internal static class CreateQuestionResponseEndpoint
{
    internal static void MapCreateQuestionResponseEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("questions/{id:guid}/response",
            async (Guid id, CreateQuestionResponseRequest request, ApplicationDbContext dbContext,
                CancellationToken cancellationToken) =>
            {
                var question = await dbContext.Questions
                    .Where(x => x.Status.Id == QuestionStatus.Published.Id ||
                                x.Status.Id == QuestionStatus.InProgress.Id)
                    .Include(question => question.Status)
                    .Include(question => question.Type)
                    .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

                if (question == null)
                {
                    return Results.NotFound();
                }

                var response = new QuestionResponse
                {
                    Username = request.Username, Question = question, Created = DateTime.Now
                };

                var rank = 1;
                foreach (var optionRanking in request.Options.OrderBy(x => x.Rank))
                {
                    var option = await dbContext.QuestionOptions
                        .FirstOrDefaultAsync(x => x.ExternalIdentifier == optionRanking.Identifier &&
                                                  x.Question.ExternalIdentifier == id, cancellationToken);

                    // Invalid data is skipped
                    if (option == null)
                    {
                        continue;
                    }

                    response.ResponseItems.Add(new QuestionResponseItem
                    {
                        Rank = question.Type.Identifier == QuestionType.RankingQuestion.Identifier ? rank++ : null,
                        QuestionResponse = response,
                        QuestionOption = option
                    });
                }

                // Set the poll to "in progress", since votes are incoming.
                if (question.Status.Id != QuestionStatus.InProgress.Id)
                {
                    question.Status = QuestionStatus.InProgress;
                }

                dbContext.Add(response);

                await dbContext.SaveChangesAsync(cancellationToken);

                return Results.Ok(new { identifier = response.ExternalIdentifier });
            });
    }

    private record CreateQuestionResponseRequest(
        string Username,
        IList<CreateQuestionResponseItemRequest> Options);

    private record CreateQuestionResponseItemRequest(Guid Identifier, int? Rank);
}
