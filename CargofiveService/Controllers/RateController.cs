using CargofiveService.Interfaces;
using CargofiveService.Models.DTOs.Internal;
using Microsoft.AspNetCore.Mvc;

namespace CargofiveService.Controllers;

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
