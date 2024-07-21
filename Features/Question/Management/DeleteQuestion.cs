using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Features.Question.Management;

internal static class DeleteQuestionEndpoint
{
    internal static void MapDeleteQuestionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("questions/{id:guid}", async (Guid id, ApplicationDbContext dbContext,
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
                .FirstOrDefaultAsync(x => x.ExternalIdentifier == id, cancellationToken);

            if (question == null)
            {
                return Results.NotFound();
            }

            dbContext.Questions.Remove(question);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        });
    }
}
