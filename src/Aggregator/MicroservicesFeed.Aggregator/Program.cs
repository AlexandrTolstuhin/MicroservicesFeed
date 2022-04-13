using MicroservicesFeed.Aggregator.Services;
using MicroservicesFeed.Shared.Redis;
using MicroservicesFeed.Shared.Redis.Streaming;
using MicroservicesFeed.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHostedService<PricingStreamBackgroundService>()
    .AddHostedService<WeatherStreamBackgroundService>()
    .AddSerialization()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming();

var app = builder.Build();

app.MapGet("/", () => "Microservices Feed Aggregator");

app.Run();