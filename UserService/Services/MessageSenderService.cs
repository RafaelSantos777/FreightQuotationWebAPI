using Azure.Messaging.ServiceBus;
using UserService.Interfaces;
using UserService.Models.DTOs.Internal;
using UserService.Models.Entities;

namespace UserService.Services;

public class MessageSenderService(
    ILogger<MessageSenderService> logger,
    ServiceBusClient serviceBusClient,
    IServiceProvider serviceProvider,
    IConfiguration configuration
) : BackgroundService {

    private DeltaLink? _currentDeltaLink;
    private ServiceBusSender _serviceBusSender = null!;
    private double _pollingIntervalMinutes;

    public override Task StartAsync(CancellationToken cancellationToken) {
        logger.LogInformation("Starting MessageSenderService");
        _serviceBusSender = serviceBusClient.CreateSender(configuration["UserDeletionQueueName"]);
        _pollingIntervalMinutes = double.Parse(configuration.GetSection("GraphAPI")["PollingIntervalMinutes"]!);
        _currentDeltaLink = null;
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
        _currentDeltaLink = await GetDeltaLink();
        while (!cancellationToken.IsCancellationRequested) {
            (ICollection<UserDTO> deletedUserDTOs, string nextDeltaLink) = await GetDeletedUsers();
            await UpdateDeltaLink(nextDeltaLink);
            await SendUserDeletionMessages(deletedUserDTOs);
            await Task.Delay(TimeSpan.FromMinutes(_pollingIntervalMinutes), cancellationToken);
        }
        return;

        async Task<DeltaLink?> GetDeltaLink() {
            await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
            IDeltaLinkRepository deltaLinkRepository = scope.ServiceProvider.GetRequiredService<IDeltaLinkRepository>();
            return await deltaLinkRepository.Get();
        }

        async Task<(ICollection<UserDTO> deletedUserDTOs, string nextDeltaLink)> GetDeletedUsers() {
            await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
            IUserQueryService userQueryService = scope.ServiceProvider.GetRequiredService<IUserQueryService>();
            return await userQueryService.GetDeletedUsers(_currentDeltaLink?.Link);
        }

        async Task UpdateDeltaLink(string deltaLink) {
            await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
            IDeltaLinkRepository deltaLinkRepository = scope.ServiceProvider.GetRequiredService<IDeltaLinkRepository>();
            if (_currentDeltaLink is null)
                _currentDeltaLink = new DeltaLink { Link = deltaLink };
            else
                _currentDeltaLink.Link = deltaLink;
            await deltaLinkRepository.AddOrUpdate(_currentDeltaLink);
        }

        async Task SendUserDeletionMessages(ICollection<UserDTO> deletedUserDTOs) {
            if (deletedUserDTOs.Count == 0)
                return;
            using ServiceBusMessageBatch messageBatch = await _serviceBusSender.CreateMessageBatchAsync(cancellationToken);
            foreach (UserDTO deletedUserDTO in deletedUserDTOs)
                messageBatch.TryAddMessage(new ServiceBusMessage(deletedUserDTO.Id));
            await _serviceBusSender.SendMessagesAsync(messageBatch, cancellationToken);
        }

    }

}
