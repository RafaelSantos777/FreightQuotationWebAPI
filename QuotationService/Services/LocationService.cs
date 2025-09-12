using QuotationService.Exceptions;
using QuotationService.Interfaces;
using QuotationService.Models.DTOs;
using QuotationService.Models.Entities;

namespace QuotationService.Services;

public class LocationService(ILocationRepository locationRepository) : ILocationService {

    public async Task<IEnumerable<LocationDTO>> Search(string? search, string? type, int page, int limit) {
        List<Location> locations;
        if (type is null)
            locations = (await locationRepository.Search<Location>(search, page, limit)).ToList();
        else if (type.Equals(nameof(Airport), StringComparison.CurrentCultureIgnoreCase))
            locations = (await locationRepository.Search<Airport>(search, page, limit)).ToList().ConvertAll<Location>(airport => airport);
        else if (type.Equals(nameof(Seaport), StringComparison.CurrentCultureIgnoreCase))
            locations = (await locationRepository.Search<Seaport>(search, page, limit)).ToList().ConvertAll<Location>(seaport => seaport);
        else
            throw new InvalidLocationException("Invalid location type.");
        return locations.ConvertAll(LocationDTO.FromLocation);
    }

    public async Task<LocationDTO?> GetById(long id) {
        Location? location = await locationRepository.GetById(id);
        return location is null ? null : LocationDTO.FromLocation(location);
    }

}
