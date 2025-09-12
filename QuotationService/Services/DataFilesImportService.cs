using QuotationService.Interfaces;
using QuotationService.Models.Entities;

namespace QuotationService.Services;

public class DataFilesImportService(
    ILogger<DataFilesImportService> logger,
    IServiceProvider serviceProvider,
    IConfiguration configuration
) : BackgroundService {

    private string _csvSeparator = null!;
    private string _airportsFilePath = null!;
    private string[] _airportsFieldsOrder = null!;
    private string _specialHandlingCodesFilePath = null!;
    private string[] _specialHandlingCodesFieldsOrder = null!;

    public override Task StartAsync(CancellationToken cancellationToken) {
        logger.LogInformation("Starting DataFilesImportService");
        IConfigurationSection dataFilesConfigurationSection = configuration.GetSection("DataFiles");
        _csvSeparator = dataFilesConfigurationSection["CSVSeparator"]!;
        IConfigurationSection airportsCSVConfigurationSection = dataFilesConfigurationSection.GetSection("AirportsCSV");
        _airportsFilePath = airportsCSVConfigurationSection["FilePath"]!;
        _airportsFieldsOrder = airportsCSVConfigurationSection.GetSection("FieldsOrder").Get<string[]>()!;
        IConfigurationSection specialHandlingCodesConfigurationSection = dataFilesConfigurationSection.GetSection("SpecialHandlingCodesCSV");
        _specialHandlingCodesFilePath = specialHandlingCodesConfigurationSection["FilePath"]!;
        _specialHandlingCodesFieldsOrder = specialHandlingCodesConfigurationSection.GetSection("FieldsOrder").Get<string[]>()!;
        return base.StartAsync(cancellationToken);
    }

    // TODO Only one microservice instance should run this BackgroundService
    protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
        await ImportAirports(cancellationToken);
        await ImportSpecialHandlingCodes(cancellationToken);
        await serviceProvider.GetRequiredService<ISpecialHandlingCodeCache>().InitializeCache();
        logger.LogInformation("DataFilesImportService complete");
    }


    private async Task ImportAirports(CancellationToken cancellationToken) {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        ILocationRepository repository = scope.ServiceProvider.GetRequiredService<ILocationRepository>();
        List<Airport> airports = [];
        HashSet<string> existingAirportIATACodes = (await repository.GetAllByType<Airport>()).Select(airport => airport.IATACode).ToHashSet();
        string[] lines = await File.ReadAllLinesAsync(_airportsFilePath, cancellationToken);
        foreach (string line in lines) {
            string[] fields = line.Split(_csvSeparator);
            string iataCode = fields[Array.IndexOf(_airportsFieldsOrder, "IATACode")];
            if (existingAirportIATACodes.Contains(iataCode))
                continue;
            airports.Add(new Airport {
                Name = fields[Array.IndexOf(_airportsFieldsOrder, "Name")],
                CountryCode = fields[Array.IndexOf(_airportsFieldsOrder, "CountryCode")],
                Latitude = decimal.Parse(fields[Array.IndexOf(_airportsFieldsOrder, "Latitude")]),
                Longitude = decimal.Parse(fields[Array.IndexOf(_airportsFieldsOrder, "Longitude")]),
                IATACode = iataCode
            });
        }
        if (airports.Count != 0)
            await repository.AddRange(airports);
    }

    private async Task ImportSpecialHandlingCodes(CancellationToken cancellationToken) {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        ISpecialHandlingCodeRepository repository = scope.ServiceProvider.GetRequiredService<ISpecialHandlingCodeRepository>();
        List<SpecialHandlingCode> specialHandlingCodes = [];
        HashSet<string> existingSpecialHandlingCodes = (await repository.GetAll()).Select(specialHandlingCode => specialHandlingCode.Code).ToHashSet();
        string[] lines = await File.ReadAllLinesAsync(_specialHandlingCodesFilePath, cancellationToken);
        foreach (string line in lines) {
            string[] fields = line.Split(_csvSeparator);
            string code = fields[Array.IndexOf(_specialHandlingCodesFieldsOrder, "Code")];
            if (existingSpecialHandlingCodes.Contains(code))
                continue;
            specialHandlingCodes.Add(new SpecialHandlingCode { Code = code, Description = fields[Array.IndexOf(_specialHandlingCodesFieldsOrder, "Description")] });
        }
        if (specialHandlingCodes.Count != 0)
            await repository.AddRange(specialHandlingCodes);
    }

}
