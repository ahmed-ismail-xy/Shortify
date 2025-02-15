using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shortify.Application.Configurations;
using Shortify.Domain.Abstractions.Errors;
using Shortify.Domain.Abstractions;
using Shortify.Api.Configurations;

namespace Shortify.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
public class ApiController : ControllerBase
{
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return HandleError(result.Error);
    }

    protected IActionResult HandleResult<TValue>(Result<TValue> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return HandleError(result.Error);
    }

    protected IActionResult HandleError(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            ErrorType.Validation => 400,
            ErrorType.Unauthorized => 401,
            ErrorType.Forbidden => 403,
            ErrorType.BadRequest => 400,
            ErrorType.InternalServerError => 500,
            ErrorType.ServiceUnavailable => 503,
            ErrorType.TooManyRequests => 429,
            ErrorType.UnprocessableEntity => 422,
            _ => 500
        };

        return Problem(statusCode: statusCode, title: error.Code, detail: error.Description);
    }

    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Code == "Error.Validation"))
        {
            return ValidationProblem(errors);
        }

        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        return HandleError(errors[0]);
    }

    private ActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }
        return ValidationProblem(modelStateDictionary);
    }
}
