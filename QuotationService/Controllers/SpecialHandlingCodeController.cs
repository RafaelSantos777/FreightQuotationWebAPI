using Microsoft.AspNetCore.Mvc;
using QuotationService.Interfaces;
using QuotationService.Models.DTOs;

namespace QuotationService.Controllers;

[Route("special-handling-codes")]
public class SpecialHandlingCodeController(ISpecialHandlingCodeCache specialHandlingCodeCache) : Controller {

    [HttpGet]
    [ProducesResponseType(typeof(List<SpecialHandlingCodeDTO>), StatusCodes.Status200OK)]
    public ActionResult GetAll() => Ok(specialHandlingCodeCache.GetCacheAsEnumerable());

}
