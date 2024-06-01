using Rankt.Features.RankingQuestion.Execution;
using Rankt.Features.RankingQuestion.Management;

namespace Rankt.Features.RankingQuestion;

internal static class RankingQuestionConfiguration
{
    internal static void MapRankingQuestionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var executionGroup = endpoints.MapGroup("api")
            .WithTags(["Public APIs / Allow Anonymous", "Survey Execution"])
            .RequireRateLimiting("fixed")
            .AllowAnonymous();

        executionGroup.MapGetRankingQuestionEndpoint();
        executionGroup.MapCreateRankingQuestionResponseEndpoint();
        executionGroup.MapGetRankingQuestionResultEndpoint();

        var managementGroup = endpoints.MapGroup("api/management")
            .WithTags("Survey Management")
            .RequireAuthorization();

        managementGroup.MapGetMyRankingQuestionEndpoint();
        managementGroup.MapCreateRankingQuestionEndpoint();
        managementGroup.MapDeleteRankingQuestionEndpoint();
        managementGroup.MapUpdateRankingQuestionEndpoint();
        managementGroup.MapUpdateRankingQuestionStatusEndpoint();
    }
}
