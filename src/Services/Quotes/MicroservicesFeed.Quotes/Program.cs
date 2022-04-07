using MicroservicesFeed.Quotes.Pricing.Requests;
using MicroservicesFeed.Quotes.Pricing.Services;
using MicroservicesFeed.Shared.DateProvider;
using MicroservicesFeed.Shared.Redis;
using MicroservicesFeed.Shared.Redis.Streaming;
using MicroservicesFeed.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<PricingRequestChannel>()
    .AddSingleton<IPricingService, RandomPricingService>()
    .AddHostedService<PricingBackgroundService>()
    .AddDateProvider()
    .AddSerialization()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming();

var app = builder.Build();

app.MapGet("/", () => "Microservices Feed Quotes");
app.MapPost("/pricing/start", async (PricingRequestChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StartPricing());
    return Results.Ok();
});
app.MapPost("/pricing/stop", async (PricingRequestChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StopPricing());
    return Results.Ok();
});

app.Run();