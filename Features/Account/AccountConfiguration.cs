namespace Rankt.Features.Account;

internal static class AccountConfiguration
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var publicApiGroup = endpoints.MapGroup("api/account")
            .WithTags(["Public APIs / Allow Anonymous", "Account"])
            .RequireRateLimiting("fixed")
            .AllowAnonymous();

        publicApiGroup.MapAccountLoginEndpoint();
        publicApiGroup.MapGetAccountInfoEndpoint();

        var userGroup = endpoints.MapGroup("api/account")
            .WithTags(["Account"])
            .RequireAuthorization();

        userGroup.MapAccountLogoutEndpoint();
        userGroup.MapUpdateAccountEndpoint();
    }
}
