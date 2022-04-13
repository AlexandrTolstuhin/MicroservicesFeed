using DotPulsar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesFeed.Shared.Pulsar;

public static class PulsarServiceCollectionExtensions
{
    public static IServiceCollection AddPulsar(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Pulsar");
        PulsarOptions options = new();
        section.Bind(options);
        services.Configure<PulsarOptions>(section);

        return services.AddSingleton(PulsarClient
            .Builder()
            .ServiceUrl(new Uri(options.ConnectionString))
            .Build());
    }
}