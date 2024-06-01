using Microsoft.AspNetCore.Identity;

namespace Rankt.Features.Account;

internal static class AccountLogoutEndpoint
{
    internal static void MapAccountLogoutEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("logout", async (SignInManager<IdentityUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        });
    }
}
