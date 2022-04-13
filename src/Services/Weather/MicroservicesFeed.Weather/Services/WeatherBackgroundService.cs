using MicroservicesFeed.Shared.Streaming;

namespace MicroservicesFeed.Weather.Services;

public class WeatherBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IStreamPublisher _streamPublisher;
    private readonly ILogger<WeatherBackgroundService> _logger;

    public WeatherBackgroundService(
        IServiceProvider serviceProvider,
        IStreamPublisher streamPublisher,
        ILogger<WeatherBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _streamPublisher = streamPublisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var weatherFeed = scope.ServiceProvider.GetRequiredService<IWeatherService>();

        await foreach (var weather in weatherFeed.SubscribeAsync("St. Petersburg", stoppingToken))
        {
            _logger.LogInformation("{Location}: {Temperature} C, {Humidity} %, {Wind} km/h [{Condition}]",
                weather.Location,
                weather.Temperature,
                weather.Humidity,
                weather.Wind,
                weather.Condition);

            await _streamPublisher.PublishAsync("weather", weather, stoppingToken);
        }
    }
}