using CargofiveService.Models.DTOs.Internal;

namespace CargofiveService.Interfaces;

public interface IRateService {

    Task<IEnumerable<RateDTO>> GetUpToDateRates(RateRequestDTO rateRequestDTO);

}
