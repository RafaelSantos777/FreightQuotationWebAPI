using Microsoft.AspNetCore.Mvc;
using WebCargoService.Interfaces;
using WebCargoService.Models.DTOs.Internal;

namespace WebCargoService.Controllers;

[ApiController]
[Route("[controller]s")]
public class RateController(IRateService rateService) : ControllerBase {

    [HttpPost]
    [ProducesResponseType(typeof(List<RateDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetRates([FromBody] RateRequestDTO rateRequestDTO) {
        List<RateDTO> rateDTOs = (await rateService.GetUpToDateRates(rateRequestDTO)).ToList();
        return Ok(rateDTOs);
    }

}
