using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Rankt.Extensions;

// see https://github.com/dotnet/aspnetcore/issues/50303#issuecomment-1836909245
public static class UserEndpointBuilderExtensions
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var apiGroup = endpoints.MapGroup("api")
            .DisableAntiforgery(); // TODO

        apiGroup.MapPost("/login", async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
            ([FromBody] LoginRequest login, SignInManager<IdentityUser> signInManager) =>
        {
            signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;

            var result = await signInManager.PasswordSignInAsync(login.Username, login.Password, true,
                true);

            if (!result.Succeeded)
            {
                return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
            }

            // The signInManager already produced the needed response in the form of a cookie or bearer token.
            return TypedResults.Empty;
        }).RequireRateLimiting("fixed").AllowAnonymous();

        apiGroup.MapPost("/logout", async (SignInManager<IdentityUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            })
            .WithOpenApi()
            .RequireAuthorization();

        apiGroup.MapPost("/user/info", async (ClaimsPrincipal claimsPrincipal,
            UserManager<IdentityUser> userManager) =>
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);
            return Results.Ok(new { isAuthenticated = user != null });
        }).AllowAnonymous();

        apiGroup.MapPost("/user/account", async Task<Results<Ok, ValidationProblem, NotFound>>
        (ClaimsPrincipal claimsPrincipal, [FromBody] PasswordChangeRequest request,
            UserManager<IdentityUser> userManager) =>
        {
            if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
            {
                return TypedResults.NotFound();
            }

            if (string.IsNullOrEmpty(request.NewPassword))
            {
                return TypedResults.Ok();
            }

            if (string.IsNullOrEmpty(request.OldPassword))
            {
                return CreateValidationProblem("OldPasswordRequired",
                    "The old password is required to set a new password.");
            }

            var changePasswordResult =
                await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                return CreateValidationProblem(changePasswordResult);
            }

            return TypedResults.Ok();
        }).RequireAuthorization();
    }

    private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription)
    {
        return TypedResults.ValidationProblem(new Dictionary<string, string[]> { { errorCode, [errorDescription] } });
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }

    private sealed class PasswordChangeRequest
    {
        public required string NewPassword { get; init; }
        public required string OldPassword { get; init; }
    }

    private sealed class LoginRequest
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}
