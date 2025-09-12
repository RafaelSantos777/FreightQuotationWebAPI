using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CargofiveService.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ExceptionHandlerController : ControllerBase {

    [Route("/error")]
    public ActionResult Error() {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        return exception switch {
            _ => Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected Error Occurred",
                detail: null)
        };
    }

}
