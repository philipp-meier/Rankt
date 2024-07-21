using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.Question.Management;

internal static class UpdateQuestionStatusEndpoint
{
    private static readonly (string current, string target)[] s_allowedStatusTransitions =
    [
        (QuestionStatus.New.Identifier, QuestionStatus.Published.Identifier),
        (QuestionStatus.Published.Identifier, QuestionStatus.InProgress.Identifier),
        (QuestionStatus.Published.Identifier, QuestionStatus.Completed.Identifier),
        (QuestionStatus.InProgress.Identifier, QuestionStatus.Completed.Identifier),
        (QuestionStatus.Completed.Identifier, QuestionStatus.Archived.Identifier)
    ];

    internal static void MapUpdateQuestionStatusEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("questions/{id:guid}/status", async (Guid id, UpdateQuestionStatusRequest request,
            ApplicationDbContext dbContext,
            ClaimsPrincipal claimsPrincipal, UserManager<IdentityUser> userManager,
            CancellationToken cancellationToken) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var question = await dbContext.Questions
                .Where(x => x.CreatedBy == user)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

            if (question == null)
            {
                return Results.NotFound();
            }

            var newStatus =
                await dbContext.QuestionStatus.FirstOrDefaultAsync(x => x.Identifier == request.Identifier,
                    cancellationToken);

            if (newStatus == null)
            {
                return Results.BadRequest(new { error = $"Invalid status identifier \"{request.Identifier}\"." });
            }

            var currentStatus = question.Status;

            // TODO: Improve "StateMachine"
            if (!s_allowedStatusTransitions.Any(x =>
                    x.current == currentStatus.Identifier && x.target == newStatus.Identifier))
            {
                return Results.BadRequest(new
                {
                    error = $"A status change from \"{currentStatus.Name}\" to \"{newStatus.Name}\" is not allowed."
                });
            }

            question.Status = newStatus;

            dbContext.Update(question);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        });
    }

    private record UpdateQuestionStatusRequest(string Identifier);
}
