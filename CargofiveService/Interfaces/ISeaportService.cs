using CargofiveService.Models.DTOs.Internal;

namespace CargofiveService.Interfaces;

public interface ISeaportService {

    Task<IEnumerable<SeaportDTO>> GetSeaports(int page, int limit);

}
