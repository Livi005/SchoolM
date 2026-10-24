using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Api.Extensions;

public static class ControllerExtensions
{
    public static IActionResult ToActionResult<T>(
        this ControllerBase controller, Result<T> result, bool created = false)
    {
        if (result.IsSuccess)
            return created
                ? controller.StatusCode(StatusCodes.Status201Created, result.Value)
                : controller.Ok(result.Value);

        return ToProblem(controller, result.Error);
    }

    public static IActionResult ToActionResult(this ControllerBase controller, Result result)
        => result.IsSuccess
            ? controller.NoContent()
            : ToProblem(controller, result.Error);

    private static IActionResult ToProblem(ControllerBase controller, Error error)
    {
        var status = error.Code switch
        {
            "NotFound" => StatusCodes.Status404NotFound,
            "Validation" => StatusCodes.Status400BadRequest,
            "Unauthorized" => StatusCodes.Status401Unauthorized,
            "Forbidden" => StatusCodes.Status403Forbidden,
            "Conflict" => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return controller.Problem(statusCode: status, title: error.Code, detail: error.Message);
    }
}