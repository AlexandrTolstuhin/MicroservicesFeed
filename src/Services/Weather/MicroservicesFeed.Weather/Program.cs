using MicroservicesFeed.Shared.Http;
using MicroservicesFeed.Shared.Redis;
using MicroservicesFeed.Shared.Redis.Streaming;
using MicroservicesFeed.Shared.Serialization;
using MicroservicesFeed.Weather.Models;
using MicroservicesFeed.Weather.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<WeatherServiceOptions>(builder.Configuration.GetSection("WeatherService"))
    .AddSerialization()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddHttpClient()
    .AddHttpApiClient<IWeatherService, WeatherService>(builder.Configuration)
    .AddHostedService<WeatherBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "Microservices Feed Weather");

app.Run();