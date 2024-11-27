using Rankt.Features.Question.Execution;
using Rankt.Features.Question.Management;

namespace Rankt.Features.Question;

internal static class QuestionConfiguration
{
    internal static void MapQuestionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var executionGroup = endpoints.MapGroup("api")
            .WithTags("Public APIs / Allow Anonymous", "Survey Execution")
            .RequireRateLimiting("fixed")
            .AllowAnonymous();

        executionGroup.MapGetQuestionEndpoint();
        executionGroup.MapCreateQuestionResponseEndpoint();
        executionGroup.MapGetQuestionResultEndpoint();

        var managementGroup = endpoints.MapGroup("api/management")
            .WithTags("Survey Management")
            .RequireAuthorization();

        managementGroup.MapGetMyQuestionEndpoint();
        managementGroup.MapCreateQuestionEndpoint();
        managementGroup.MapDeleteQuestionEndpoint();
        managementGroup.MapUpdateQuestionEndpoint();
        managementGroup.MapUpdateQuestionStatusEndpoint();
    }
}
