using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MicroservicesFeed.Weather.Models;
using Microsoft.Extensions.Options;

namespace MicroservicesFeed.Weather.Services;

internal class WeatherService : IWeatherService
{
    private readonly HttpClient _client;
    private readonly WeatherServiceOptions _options;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(HttpClient client, IOptions<WeatherServiceOptions> options, ILogger<WeatherService> logger)
    {
        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    public async IAsyncEnumerable<WeatherData> SubscribeAsync(
        string location,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var url = $"{_options.ApiUrl}?key={_options.ApiKey}&q={location}&aqi=no";

        while (!cancellationToken.IsCancellationRequested)
        {
            WeatherApiResponse? response;

            try
            {
                response = await _client.GetFromJsonAsync<WeatherApiResponse>(url, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            if (response is null)
            {
                await Task.Delay(TimeSpan.FromSeconds(_options.RequestDelay), cancellationToken);
                continue;
            }

            yield return new WeatherData($"{response.Location.Name}, {response.Location.Country}",
                response.Current.TempC, response.Current.Humidity, response.Current.WindKph,
                response.Current.Condition.Text);

            await Task.Delay(TimeSpan.FromSeconds(_options.RequestDelay), cancellationToken);
        }
    }

    private record WeatherApiResponse(Location Location, Weather Current);

    private record Location(string Name, string Country);

    private record Condition(string Text);

    private record Weather(
        [property: JsonPropertyName("temp_c")] double TempC,
        double Humidity,
        Condition Condition,
        [property: JsonPropertyName("wind_kph")] double WindKph);
}