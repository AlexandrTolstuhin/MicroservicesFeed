using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace MicroservicesFeed.Shared.Http;

public static class HttpClientServiceCollectionCollectionExtensions
{
    public static IServiceCollection AddHttpApiClient<TInterface, TClient>(
        this IServiceCollection services,
        IConfiguration configuration) 
        where TInterface : class where TClient : class, TInterface
    {
        var section = configuration.GetRequiredSection("HttpApiClient");
        var options = new HttpApiClientOptions();
        section.Bind(options);
        services.Configure<HttpApiClientOptions>(section);

        services
            .AddHttpClient<TInterface, TClient>()
            .AddPolicyHandler(GetPolicy(options));

        return services;

        static IAsyncPolicy<HttpResponseMessage> GetPolicy(HttpApiClientOptions options)
            => HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(options.RetryCount, retry => TimeSpan.FromSeconds(2 << retry));
    }
}