using Microsoft.AspNetCore.Mvc;
using Randomizer.Common;

namespace Randomizer.Api.Extensions;
public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccessful)
        {
            return new NoContentResult();
        }

        return HandleErrorResult(result);
    }

    public static ActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccessful)
        {
            return new OkObjectResult(result.PayLoad);
        }

        return HandleErrorResult(result);
    }

    private static ActionResult HandleErrorResult(Result result)
    {
        if (result.ValidationErrors.Any())
        {
            return new BadRequestObjectResult(result.ValidationErrors);
        }

        return result.ApiErrorCode switch
        {
            ApiErrorCodes.Unauthorized => new UnauthorizedObjectResult(result.ErrorMessage),
            ApiErrorCodes.NotFound => new NotFoundObjectResult(result.ErrorMessage),
            _ => new BadRequestObjectResult(result.ErrorMessage)
        };
    }
}
