using Grpc.Core;
using Grpc.Net.Client;
using MicroservicesFeed.Clients.Console;

using var channel = GrpcChannel.ForAddress("https://localhost:7000/");
var client = new PricingFeed.PricingFeedClient(channel);

var symbolsResponse = await client.GetSymbolsAsync(new SymbolsRequest());
foreach (var symbol in symbolsResponse.Symbols)
{
    Console.WriteLine(symbol);
}

var pricingStream = client.SubscribePricing(new PricingRequest
{
    Symbol = "USDRUB"
});

await foreach (var price in pricingStream.ResponseStream.ReadAllAsync())
{
    Console.WriteLine(
        $"{DateTimeOffset.FromUnixTimeMilliseconds(price.Timestamp):T} -> {price.Symbol} = {price.Value / 100M:F}");
}