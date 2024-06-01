using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Rankt.Features.Account;

internal static class AccountLoginEndpoint
{
    internal static void MapAccountLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("login", async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
            ([FromBody] LoginRequest login, SignInManager<IdentityUser> signInManager) =>
        {
            signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;

            var result = await signInManager.PasswordSignInAsync(login.Username, login.Password, true,
                true);

            if (!result.Succeeded)
            {
                // Generic error message without hinting whether the credentials were wrong or the user exists/is locked.
                return TypedResults.Problem("Login failed.", statusCode: StatusCodes.Status401Unauthorized);
            }

            // The signInManager already produced the needed response in the form of a cookie or bearer token.
            return TypedResults.Empty;
        });
    }

    private sealed record LoginRequest(string Username, string Password);
}
