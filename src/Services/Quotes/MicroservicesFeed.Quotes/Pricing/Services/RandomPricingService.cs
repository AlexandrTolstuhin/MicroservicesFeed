using System.Runtime.CompilerServices;
using MicroservicesFeed.Shared.DateProvider;

namespace MicroservicesFeed.Quotes.Pricing.Services;

internal class RandomPricingService : IPricingService
{
    private readonly IDateProvider _dateProvider;
    private readonly ILogger<RandomPricingService> _logger;
    private readonly Random _random = new();

    private readonly Dictionary<string, decimal> _symbolPrices = new()
    {
        ["EURUSD"] = 1.1M,
        ["EURRUB"] = 86M,
        ["USDRUB"] = 80M
    };

    public RandomPricingService(IDateProvider dateProvider, ILogger<RandomPricingService> logger)
    {
        _dateProvider = dateProvider;
        _logger = logger;
    }

    public IEnumerable<string> GetSymbols() => _symbolPrices.Keys;

    public async IAsyncEnumerable<CurrencyPair> GetPrices(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            foreach (var (symbol, price) in _symbolPrices)
            {
                var tick = NextTick();
                var newPrice = price + tick;
                _symbolPrices[symbol] = newPrice;

                _logger.LogInformation(
                    "Updated pricing for: {Symbol}, {Price:F} -> {NewPrice:F} [{Tick:F}]",
                    symbol,
                    price,
                    newPrice,
                    tick);

                var currencyPair = new CurrencyPair(symbol, newPrice, _dateProvider.UtcNow.ToUnixTimeMilliseconds());
                PriceChanged?.Invoke(this, currencyPair);
                yield return currencyPair;
            }

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

    public event EventHandler<CurrencyPair>? PriceChanged;

    private decimal NextTick()
    {
        var sign = _random.Next(0, 2) == 0 ? -1 : 1;
        var tick = _random.NextDouble() / 10;
        return (decimal) (sign * tick);
    }
}