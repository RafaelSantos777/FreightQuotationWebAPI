using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using QuotationService.Interfaces;
using QuotationService.Models.DTOs.Internal;

namespace QuotationService.Controllers;

[Route("quotes/air")]
public class AirQuotationController(IAirQuotationService airQuotationService) : Controller {

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AirQuoteResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAirQuote([FromRoute] long id) {
        AirQuoteResponseDTO airQuoteResponseDTO = await airQuotationService.GetAirQuoteResponse(id, User.GetObjectId()!);
        if (User.GetObjectId() != airQuoteResponseDTO.AirQuoteRequestDetailed.UserId)
            return Forbid();
        return Ok(airQuoteResponseDTO);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AirQuoteResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> CreateAirQuotes([FromBody] AirQuoteRequestDTO airQuoteRequestDTO) {
        AirQuoteResponseDTO airQuoteResponseDTO = await airQuotationService.CreateAirQuoteResponse(airQuoteRequestDTO, User.GetObjectId()!);
        return CreatedAtAction(nameof(GetAirQuote), new { id = airQuoteResponseDTO.Id }, airQuoteResponseDTO);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAirQuote([FromRoute] long id) {
        await airQuotationService.DeleteAirQuoteResponse(id, User.GetObjectId()!);
        return NoContent();
    }

    [HttpGet("history")]
    [ProducesResponseType(typeof(List<AirQuoteResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> GetAirQuoteHistory([FromQuery] int page = 1, [FromQuery] int limit = 100) {
        List<AirQuoteResponseDTO> airQuoteHistory = (await airQuotationService.GetUserHistory(User.GetObjectId()!, page, limit)).ToList();
        return Ok(airQuoteHistory);
    }

}
