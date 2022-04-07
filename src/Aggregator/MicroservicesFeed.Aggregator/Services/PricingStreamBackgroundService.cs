using MicroservicesFeed.Aggregator.Services.Models;
using MicroservicesFeed.Shared.Streaming;

namespace MicroservicesFeed.Aggregator.Services;

internal sealed class PricingStreamBackgroundService : BackgroundService
{
    private readonly IStreamSubscriber _subscriber;
    private readonly ILogger<PricingStreamBackgroundService> _logger;

    public PricingStreamBackgroundService(
        IStreamSubscriber subscriber,
        ILogger<PricingStreamBackgroundService> logger)
    {
        _subscriber = subscriber;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _subscriber.SubscribeAsync<CurrencyPair>("pricing", currencyPair =>
        {
            _logger.LogInformation(
                "Price '{Symbol}' = {Value:F}, timestamp: {Timestamp}",
                currencyPair.Symbol,
                currencyPair.Value,
                currencyPair.Timestamp);
        }, stoppingToken);
    }
}