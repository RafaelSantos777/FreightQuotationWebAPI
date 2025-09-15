using Azure.Messaging.ServiceBus;
using QuotationService.Interfaces;

namespace QuotationService.Services;

public class MessageReceiverService(
    ILogger<MessageReceiverService> logger,
    ServiceBusClient serviceBusClient,
    IServiceProvider serviceProvider,
    IConfiguration configuration
) : BackgroundService {

    private ServiceBusProcessor _serviceBusProcessor = null!;

    public override Task StartAsync(CancellationToken cancellationToken) {
        logger.LogInformation("Starting MessageReceiverService");
        _serviceBusProcessor = serviceBusClient.CreateProcessor(configuration["UserDeletionTopicName"], configuration["TopicSubscriptionName"]);
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
        _serviceBusProcessor.ProcessMessageAsync += ProcessMessage;
        _serviceBusProcessor.ProcessErrorAsync += ProcessMessageError;
        await _serviceBusProcessor.StartProcessingAsync(cancellationToken);
    }

    // TODO Delete user history from ISeaQuoteRepository as well
    private async Task ProcessMessage(ProcessMessageEventArgs messageEventArgs) {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        IAirQuoteRepository airQuoteRepository = scope.ServiceProvider.GetRequiredService<IAirQuoteRepository>();
        string userId = messageEventArgs.Message.Body.ToString();
        await airQuoteRepository.DeleteUserHistory(userId);
    }

    private Task ProcessMessageError(ProcessErrorEventArgs errorEventArgs) {
        logger.LogError(errorEventArgs.Exception, "Error processing message. Source: {ErrorSource}, Entity: {EntityPath}", errorEventArgs.ErrorSource, errorEventArgs.EntityPath);
        return Task.CompletedTask;
    }

}
