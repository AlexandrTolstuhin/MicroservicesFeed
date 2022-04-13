using MicroservicesFeed.Shared.Streaming;

namespace MicroservicesFeed.Aggregator.Services;

public class WeatherStreamBackgroundService : BackgroundService
{
    private readonly IStreamSubscriber _subscriber;
    private readonly ILogger<WeatherStreamBackgroundService> _logger;

    public WeatherStreamBackgroundService(IStreamSubscriber subscriber, ILogger<WeatherStreamBackgroundService> logger)
    {
        _subscriber = subscriber;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _subscriber.SubscribeAsync<WeatherData>("weather", data =>
        {
            _logger.LogInformation("{Location}: {Temperature} C, {Humidity} %, {Wind} km/h [{Condition}]",
                data.Location,
                data.Temperature,
                data.Humidity,
                data.Wind,
                data.Condition);

        }, stoppingToken);
    }

    private record WeatherData(string Location, double Temperature, double Humidity, double Wind, string Condition);
}