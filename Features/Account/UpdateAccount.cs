using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Rankt.Features.Account;

internal static class UpdateAccountEndpoint
{
    internal static void MapUpdateAccountEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async Task<Results<Ok, ValidationProblem, NotFound>>
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
        });
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

    private sealed record PasswordChangeRequest(string NewPassword, string OldPassword);
}
