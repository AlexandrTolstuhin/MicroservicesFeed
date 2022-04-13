using MicroservicesFeed.Aggregator.Events;
using MicroservicesFeed.Aggregator.Services.Models;
using MicroservicesFeed.Shared.Messaging;

namespace MicroservicesFeed.Aggregator.Services;

internal class PricingHandler : IPricingHandler
{
    private int _counter;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<PricingHandler> _logger;

    public PricingHandler(IMessagePublisher messagePublisher, ILogger<PricingHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task HandleAsync(CurrencyPair currencyPair, CancellationToken cancellationToken = default)
    {
        //TODO: Implement some actual business logic
        if (ShouldPlaceOrder())
        {
            var orderId = Guid.NewGuid().ToString("N");

            _logger.LogInformation(
                "Order with ID: {OrderId} has been placed for symbol: '{Symbol}'",
                orderId,
                currencyPair.Symbol);

            OrderPlaced integrationEvent = new(orderId, currencyPair.Symbol);
            await _messagePublisher.PublishAsync("orders", integrationEvent, cancellationToken);
        }
    }

    private bool ShouldPlaceOrder() => Interlocked.Increment(ref _counter) % 10 == 0;
}