using WebCargoService.Models.DTOs.Internal;

namespace WebCargoService.Interfaces;

public interface IRateService {

    Task<IEnumerable<RateDTO>> GetUpToDateRates(RateRequestDTO rateRequestDTO);

}
