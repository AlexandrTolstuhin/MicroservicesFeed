using Grpc.Net.Client;
using MicroservicesFeed.Clients.Console;

using var channel = GrpcChannel.ForAddress("https://localhost:7000/");
var client = new PricingFeed.PricingFeedClient(channel);

var symbolsResponse = await client.GetSymbolsAsync(new SymbolsRequest());
foreach (var symbol in symbolsResponse.Symbols)
{
    Console.WriteLine(symbol);
}

Console.ReadLine();