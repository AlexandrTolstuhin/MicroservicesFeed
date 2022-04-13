using MicroservicesFeed.Notifier.Events;
using MicroservicesFeed.Shared.Messaging;

namespace MicroservicesFeed.Notifier.Services;

internal class NotifierMessagingBackgroundService : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly ILogger<NotifierMessagingBackgroundService> _logger;

    public NotifierMessagingBackgroundService(IMessageSubscriber messageSubscriber,
        ILogger<NotifierMessagingBackgroundService> logger)
    {
        _messageSubscriber = messageSubscriber;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageSubscriber.SubscribeAsync<OrderPlaced>("orders", message =>
        {
            //TODO: Implement some real notification
            _logger.LogInformation(
                "Order with ID: '{OrderId}' for symbol: '{Symbol}' has been placed",
                message.OrderId,
                message.Symbol);
        }, stoppingToken);

        return Task.CompletedTask;
    }
}