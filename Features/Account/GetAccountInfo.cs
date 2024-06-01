using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Rankt.Features.Account;

internal static class GetAccountInfoEndpoint
{
    internal static void MapGetAccountInfoEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("info", async (ClaimsPrincipal claimsPrincipal,
            UserManager<IdentityUser> userManager) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);
            return Results.Ok(new { isAuthenticated = user != null });
        }).DisableRateLimiting();
    }
}
