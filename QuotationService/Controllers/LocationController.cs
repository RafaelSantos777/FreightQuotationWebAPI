using Microsoft.AspNetCore.Mvc;
using QuotationService.Interfaces;
using QuotationService.Models.DTOs;

namespace QuotationService.Controllers;

[Route("[controller]s")]
public class LocationController(ILocationService locationService) : Controller {

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LocationDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById([FromRoute] long id) {
        LocationDTO? locationDTO = await locationService.GetById(id);
        return locationDTO is null ? NotFound() : Ok(locationDTO);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<LocationDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Search([FromQuery] string? search, [FromQuery] string? type, [FromQuery] int page = 1, [FromQuery] int limit = 100) {
        IEnumerable<LocationDTO> locationDTOs = await locationService.Search(search, type, page, limit);
        return Ok(locationDTOs);
    }

}
