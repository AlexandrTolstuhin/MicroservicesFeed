namespace MicroservicesFeed.Quotes.Pricing.Services;

internal interface IPricingService
{
    IEnumerable<string> GetSymbols();

    IAsyncEnumerable<CurrencyPair> GetPrices(CancellationToken cancellationToken = default);
}