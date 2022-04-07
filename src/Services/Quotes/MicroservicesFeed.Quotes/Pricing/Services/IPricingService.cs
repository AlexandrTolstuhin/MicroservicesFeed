namespace MicroservicesFeed.Quotes.Pricing.Services;

internal interface IPricingService
{
    IAsyncEnumerable<CurrencyPair> GetPrices(CancellationToken cancellationToken = default);
}