using MicroservicesFeed.Aggregator.Services.Models;

namespace MicroservicesFeed.Aggregator.Services;

internal interface IPricingHandler
{
    Task HandleAsync(CurrencyPair currencyPair, CancellationToken cancellationToken = default);
}