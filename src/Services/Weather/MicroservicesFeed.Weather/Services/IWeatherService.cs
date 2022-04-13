using MicroservicesFeed.Weather.Models;

namespace MicroservicesFeed.Weather.Services;

internal interface IWeatherService
{
    IAsyncEnumerable<WeatherData> SubscribeAsync(string location, CancellationToken cancellationToken = default);
}