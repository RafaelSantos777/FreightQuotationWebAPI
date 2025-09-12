using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuotationService.Exceptions;

namespace QuotationService.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ExceptionHandlerController : ControllerBase {

    [Route("/error")]
    public ActionResult Error() {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        return exception switch {
            ForbiddenAccessException ex => Problem(
                statusCode: StatusCodes.Status403Forbidden,
                title: "Access Forbidden",
                detail: ex.Message),
            InvalidLocationException ex => Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid Location",
                detail: ex.Message),
            InvalidSpecialHandlingCodeException ex => Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid Special Handling Code",
                detail: ex.Message),
            QuoteNotFoundException ex => Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Quote Not Found",
                detail: ex.Message),
            _ => Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected Error Occurred",
                detail: null)
        };
    }

}
