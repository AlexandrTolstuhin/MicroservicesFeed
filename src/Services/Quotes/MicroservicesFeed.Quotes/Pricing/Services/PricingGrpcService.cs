using Grpc.Core;

namespace MicroservicesFeed.Quotes.Pricing.Services;

internal class PricingGrpcService : PricingFeed.PricingFeedBase
{
    private readonly IPricingService _pricingService;

    public PricingGrpcService(IPricingService pricingService)
    {
        _pricingService = pricingService;
    }

    public override Task<SymbolsResponse> GetSymbols(SymbolsRequest request, ServerCallContext context)
        => Task.FromResult(new SymbolsResponse
        {
            Symbols = {_pricingService.GetSymbols()}
        });
}