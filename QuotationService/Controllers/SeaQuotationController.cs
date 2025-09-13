using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using QuotationService.Interfaces;
using QuotationService.Models.DTOs.Internal;

namespace QuotationService.Controllers;

[Route("quotes/sea")]
public class SeaQuotationController(ISeaQuotationService seaQuotationService) : Controller {

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SeaQuoteResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetSeaQuote([FromRoute] long id) {
        SeaQuoteResponseDTO seaQuoteResponseDTO = await seaQuotationService.GetSeaQuoteResponse(id, User.GetObjectId()!);
        if (User.GetObjectId() != seaQuoteResponseDTO.SeaQuoteRequestDetailed.UserId)
            return Forbid();
        return Ok(seaQuoteResponseDTO);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SeaQuoteResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> CreateSeaQuotes([FromBody] SeaQuoteRequestDTO seaQuoteRequestDTO) {
        SeaQuoteResponseDTO seaQuoteResponseDTO = await seaQuotationService.CreateSeaQuoteResponse(seaQuoteRequestDTO, User.GetObjectId()!);
        return CreatedAtAction(nameof(GetSeaQuote), new { id = seaQuoteResponseDTO.Id }, seaQuoteResponseDTO);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteSeaQuote([FromRoute] long id) {
        await seaQuotationService.DeleteSeaQuoteResponse(id, User.GetObjectId()!);
        return NoContent();
    }

    [HttpGet("history")]
    [ProducesResponseType(typeof(List<SeaQuoteResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> GetSeaQuoteHistory([FromQuery] int page = 1, [FromQuery] int limit = 100) {
        List<SeaQuoteResponseDTO> seaQuoteHistory = (await seaQuotationService.GetUserHistory(User.GetObjectId()!, page, limit)).ToList();
        return Ok(seaQuoteHistory);
    }

}
