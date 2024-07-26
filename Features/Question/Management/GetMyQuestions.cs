using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.Question.Management;

internal static class GetMyQuestionEndpoint
{
    internal static void MapGetMyQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("questions", async (ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal,
            UserManager<IdentityUser> userManager, CancellationToken cancellationToken) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var questions = await dbContext.Questions
                .Where(x => x.CreatedBy == user)
                .Include(x => x.Status)
                .Include(x => x.Responses)
                .Include(question => question.Type)
                .OrderBy(x => x.Title)
                .ToListAsync(cancellationToken);

            return new GetMyQuestionResponse
            {
                Questions = questions.Select(x => new QuestionSummary
                {
                    Identifier = x.ExternalIdentifier,
                    Title = x.Title,
                    Created = x.Created,
                    Status = x.Status.Name,
                    Type = x.Type.Identifier,
                    ResponseCount = x.Responses.Count
                }).ToList(),
                AvailableStatusOptions = dbContext.QuestionStatus.Select(x =>
                    new QuestionStatusOption(x.Identifier, x.Name)).ToList(),
                AvailableTypeOptions = dbContext.QuestionType.Select(x =>
                    new QuestionTypeOption(x.Identifier, x.Name)).ToList()
            };
        });
    }

    private record GetMyQuestionResponse
    {
        public required IList<QuestionSummary> Questions { get; set; }
        public required IList<QuestionStatusOption> AvailableStatusOptions { get; set; }
        public required IList<QuestionTypeOption> AvailableTypeOptions { get; set; }
    }

    private record QuestionSummary
    {
        public Guid Identifier { get; set; }
        public required string Title { get; set; }
        public required string Status { get; set; }
        public required string Type { get; set; }
        public int ResponseCount { get; set; }
        public DateTime Created { get; set; }
    }

    private record QuestionStatusOption(string Identifier, string Name);

    private record QuestionTypeOption(string Identifier, string Name);
}
