using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Api.Shared;


[ApiController]
public class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleErrors(List<Error> errors)
    {
        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}