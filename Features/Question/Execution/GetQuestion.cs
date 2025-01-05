using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.Question.Execution;

internal static class GetQuestionEndpoint
{
    internal static void MapGetQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapMethods("questions/{id:guid}", ["HEAD"], async (Guid id,
            ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal, UserManager<IdentityUser> userManager,
            CancellationToken cancellationToken) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);

            var question = await dbContext.Questions
                .Where(x => x.CreatedBy == user || (x.Status.Id != QuestionStatus.New.Id &&
                                                    x.Status.Id != QuestionStatus.Archived.Id))
                .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

            return question != null ? Results.Ok() : Results.NotFound();
        });

        app.MapGet("questions/{id:guid}", async (Guid id, ApplicationDbContext dbContext,
            ClaimsPrincipal claimsPrincipal, UserManager<IdentityUser> userManager,
            CancellationToken cancellationToken) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);

            var question = await dbContext.Questions
                .Where(x => x.CreatedBy == user || (x.Status.Id != QuestionStatus.New.Id &&
                                                    x.Status.Id != QuestionStatus.Archived.Id))
                .Include(question => question.Status)
                .Include(question => question.Type)
                .Include(question => question.Responses).ThenInclude(questionResponse => questionResponse.ResponseItems)
                .ThenInclude(questionResponseItem => questionResponseItem.QuestionOption)
                .FirstAsync(x => x.ExternalIdentifier == id, cancellationToken);

            var includeResponses = question.Type.Identifier == QuestionType.Voting.Identifier;
            return new GetQuestionResponse
            {
                Identifier = id,
                Title = question.Title,
                Status = question.Status.Name,
                Type = question.Type.Identifier,
                ResponseCount = question.Responses.Count,
                MaxSelectableItems = question.MaxSelectableItems,
                Created = DateTime.SpecifyKind(question.Created, DateTimeKind.Utc),
                Options = question.Options.Select(x => new GetQuestionOptionResponse
                {
                    Identifier = x.ExternalIdentifier,
                    Title = x.Title,
                    Position = x.Position,
                    Description = x.Description
                }).OrderBy(x => x.Position).ToList(),
                Responses = includeResponses
                    ? question.Responses.Select(r => new GetQuestionVoterResponse
                    {
                        Identifier = r.ExternalIdentifier,
                        Username = r.Username,
                        Choice = r.ResponseItems.Select(ri => ri.QuestionOption.ExternalIdentifier).ToArray()
                    }).OrderBy(x => x.Username).ToList()
                    : null
            };
        });
    }

    private record GetQuestionResponse
    {
        public Guid Identifier { get; set; }
        public required string Title { get; set; }
        public required string Status { get; set; }
        public required string Type { get; set; }
        public required int ResponseCount { get; set; }
        public required DateTime Created { get; set; }
        public int? MaxSelectableItems { get; set; }
        public IList<GetQuestionOptionResponse> Options { get; set; } = [];

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<GetQuestionVoterResponse>? Responses { get; set; } = [];
    }

    private record GetQuestionOptionResponse
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int Position { get; set; }
        public Guid Identifier { get; set; }
    }

    private record GetQuestionVoterResponse
    {
        public required Guid Identifier { get; set; }
        public required string Username { get; set; }
        public Guid[] Choice { get; set; } = [];
    }
}
