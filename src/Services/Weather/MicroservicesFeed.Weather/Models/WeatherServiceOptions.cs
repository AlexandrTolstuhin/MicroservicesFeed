namespace MicroservicesFeed.Weather.Models;

internal class WeatherServiceOptions
{
    public string ApiUrl { get; init; } = string.Empty;

    public string ApiKey { get; init; } = string.Empty;

    public int RequestDelay { get; init; }
}