using System.Threading.Channels;
using Grpc.Core;

namespace MicroservicesFeed.Quotes.Pricing.Services;

internal class PricingGrpcService : PricingFeed.PricingFeedBase
{
    private readonly IPricingService _pricingService;
    private readonly ILogger<PricingGrpcService> _logger;

    public PricingGrpcService(IPricingService pricingService, ILogger<PricingGrpcService> logger)
    {
        _pricingService = pricingService;
        _logger = logger;
    }

    public override Task<SymbolsResponse> GetSymbols(SymbolsRequest request, ServerCallContext context)
        => Task.FromResult(new SymbolsResponse
        {
            Symbols = {_pricingService.GetSymbols()}
        });

    public override async Task SubscribePricing(
        PricingRequest request,
        IServerStreamWriter<PricingResponse> responseStream,
        ServerCallContext context)
    {
        _logger.LogInformation("Started client streaming...");

        UnboundedChannelOptions options = new()
        {
            SingleReader = true
        };
        var channel = Channel.CreateUnbounded<CurrencyPair>(options);

        _pricingService.PriceChanged += OnPriceChanged;

        try
        {
            await foreach (var (symbol, value, timestamp) in channel.Reader.ReadAllAsync(context.CancellationToken))
            {
                if (!symbol.Equals(request.Symbol, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                await responseStream.WriteAsync(new PricingResponse
                {
                    Symbol = symbol,
                    Value = (int) (100 * value),
                    Timestamp = timestamp
                });
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Сlient requested to stop the stream...");
        }
        finally
        {
            _pricingService.PriceChanged -= OnPriceChanged;

            _logger.LogInformation("Stopped client streaming...");
        }

        void OnPriceChanged(object? sender, CurrencyPair currencyPair) => channel.Writer.TryWrite(currencyPair);
    }
}