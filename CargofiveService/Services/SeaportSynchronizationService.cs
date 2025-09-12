using CargofiveService.Interfaces;
using CargofiveService.Models.DTOs.Internal;

namespace CargofiveService.Services;

public class SeaportSynchronizationService(
    ILogger<SeaportSynchronizationService> logger,
    IServiceProvider serviceProvider,
    IConfiguration configuration
) : BackgroundService {

    private double _taskDelayDays;
    private double _apiCallDelaySeconds;

    public override Task StartAsync(CancellationToken cancellationToken) {
        logger.LogInformation("Starting SeaportSynchronizationService");
        IConfigurationSection configurationSection = configuration.GetSection("DataSynchronization");
        _taskDelayDays = double.Parse(configurationSection["SynchronizationDelayDays"]!);
        _apiCallDelaySeconds = double.Parse(configurationSection["APICallsDelaySeconds"]!);
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested) {
            await SynchronizeSeaportData();
            logger.LogInformation("Seaport Synchronization complete, waiting {TaskDelayDays} day(s) until next synchronization", _taskDelayDays);
            await Task.Delay(TimeSpan.FromDays(_taskDelayDays), cancellationToken);
        }
    }

    // TODO Only one microservice instance should run this BackgroundService
    // TODO Send to ServiceBus
    private async Task SynchronizeSeaportData() {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        ISeaportService seaportService = scope.ServiceProvider.GetRequiredService<ISeaportService>();
        int currentPage = 1;
        while (true) {
            List<SeaportDTO> cargoFiveSeaportDTOs = (await seaportService.GetSeaports(currentPage, 200)).ToList();
            if (cargoFiveSeaportDTOs.Count == 0)
                break;
            currentPage++;
            logger.LogInformation("Page {CurrentPage} of Seaports Synchronized. Waiting {ApiCallDelaySeconds} second(s).", currentPage, _apiCallDelaySeconds);
            await Task.Delay(TimeSpan.FromSeconds(_apiCallDelaySeconds));
        }

    }

}
