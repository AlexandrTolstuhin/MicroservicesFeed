using MicroservicesFeed.Aggregator.Services;
using MicroservicesFeed.Shared.Pulsar;
using MicroservicesFeed.Shared.Pulsar.Messaging;
using MicroservicesFeed.Shared.Redis;
using MicroservicesFeed.Shared.Redis.Streaming;
using MicroservicesFeed.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHostedService<PricingStreamBackgroundService>()
    .AddHostedService<WeatherStreamBackgroundService>()
    .AddSerialization()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddPulsar(builder.Configuration)
    .AddPulsarMessaging()
    .AddSingleton<IPricingHandler, PricingHandler>();

var app = builder.Build();

app.MapGet("/", () => "Microservices Feed Aggregator");

app.Run();