using System.Security.Claims;
using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.RankingQuestion;

public class GetRankingQuestionModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // TODO: Rate Limiting
        app.MapMethods("api/questions/{id:guid}", ["HEAD"], async (Guid id,
            ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal, UserManager<IdentityUser> userManager,
            CancellationToken cancellationToken) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);

            var rankingQuestion = await dbContext.RankingQuestions
                .Where(x => x.CreatedBy == user || (x.Status.Id != RankingQuestionStatus.New.Id &&
                                                    x.Status.Id != RankingQuestionStatus.Archived.Id))
                .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

            return rankingQuestion != null ? Results.Ok() : Results.NotFound();
        }).AllowAnonymous();

        app.MapGet("api/questions/{id:guid}", async (Guid id, ApplicationDbContext dbContext,
            ClaimsPrincipal claimsPrincipal, UserManager<IdentityUser> userManager,
            CancellationToken cancellationToken) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);

            var rankingQuestion = await dbContext.RankingQuestions
                .Where(x => x.CreatedBy == user || (x.Status.Id != RankingQuestionStatus.New.Id &&
                                                    x.Status.Id != RankingQuestionStatus.Archived.Id))
                .FirstAsync(x => x.ExternalIdentifier == id, cancellationToken);

            return new GetRankingQuestionResponse
            {
                Identifier = id,
                Title = rankingQuestion.Title,
                MaxSelectableItems = rankingQuestion.MaxSelectableItems,
                Created = rankingQuestion.Created,
                Options = rankingQuestion.Options.Select(x => new GetRankingQuestionOptionResponse
                {
                    Identifier = x.ExternalIdentifier, Title = x.Title, Description = x.Description
                }).ToList()
            };
        }).AllowAnonymous();
    }
}

public record GetRankingQuestionResponse
{
    public Guid Identifier { get; set; }
    public required string Title { get; set; }
    public required DateTime Created { get; set; }
    public int? MaxSelectableItems { get; set; }
    public IList<GetRankingQuestionOptionResponse> Options { get; set; } = [];
}

public record GetRankingQuestionOptionResponse
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public Guid Identifier { get; set; }
}
