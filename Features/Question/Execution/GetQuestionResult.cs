using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.Question.Execution;

internal static class GetQuestionResultEndpoint
{
    internal static void MapGetQuestionResultEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapMethods("questions/{id:guid}/result", ["HEAD"],
            async (Guid id, ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal,
                UserManager<IdentityUser> userManager, CancellationToken cancellationToken) =>
            {
                var user = await userManager.GetUserAsync(claimsPrincipal);

                var question = await dbContext.Questions
                    .Where(x => x.Status.Id == QuestionStatus.Completed.Id ||
                                (x.CreatedBy == user && x.Status.Id == QuestionStatus.Archived.Id))
                    .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

                return question != null ? Results.Ok() : Results.NotFound();
            });

        app.MapGet("questions/{id:guid}/result",
            async (Guid id, ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal,
                UserManager<IdentityUser> userManager, CancellationToken cancellationToken) =>
            {
                var user = await userManager.GetUserAsync(claimsPrincipal);

                var question = await dbContext.Questions
                    .Where(x => x.Status.Id == QuestionStatus.Completed.Id ||
                                (x.CreatedBy == user && x.Status.Id == QuestionStatus.Archived.Id))
                    .Include(x => x.Type)
                    .Include(x => x.Responses)
                    .ThenInclude(x => x.ResponseItems)
                    .ThenInclude(x => x.QuestionOption)
                    .FirstAsync(x => x.ExternalIdentifier == id, cancellationToken);

                var maxItems = question.MaxSelectableItems ?? question.Options.Count;
                var responses = question.Responses.ToArray();

                var result = new Dictionary<Guid, int>(); // Identifier, Points
                foreach (var response in responses)
                {
                    var items = response.ResponseItems
                        .OrderBy(x => x.Rank)
                        .Select(x => x.QuestionOption)
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


                var includeResponses = question.Type.Identifier == "V";

                return new GetQuestionResultResponse
                {
                    ResponseCount = question.Responses.Count,
                    Items = result.OrderByDescending(x => x.Value).Select(kv =>
                    {
                        var option = question.Options.First(x => x.ExternalIdentifier == kv.Key);
                        return new GetQuestionResultItemResponse
                        {
                            Title = option.Title, Description = option.Description, Score = kv.Value
                        };
                    }).OrderBy(x => x.Title).ToList(),
                    Responses = includeResponses
                        ? question.Responses.Select(r => new GetQuestionResultVoterResponse
                        {
                            Identifier = r.ExternalIdentifier,
                            Username = r.Username,
                            Choice = r.ResponseItems.Select(ri => ri.QuestionOption.ExternalIdentifier).ToArray()
                        }).OrderBy(x => x.Username).ToList()
                        : null
                };
            });
    }

    private record GetQuestionResultResponse
    {
        public IList<GetQuestionResultItemResponse> Items { get; set; } = [];
        public required int ResponseCount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<GetQuestionResultVoterResponse>? Responses { get; set; } = [];
    }

    private record GetQuestionResultItemResponse
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int Score { get; set; }
    }

    private record GetQuestionResultVoterResponse
    {
        public required Guid Identifier { get; set; }
        public required string Username { get; set; }
        public Guid[] Choice { get; set; } = [];
    }
}
