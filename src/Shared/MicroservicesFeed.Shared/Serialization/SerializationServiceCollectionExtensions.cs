using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesFeed.Shared.Serialization;

public static class SerializationServiceCollectionExtensions
{
    public static IServiceCollection AddSerialization(this IServiceCollection services)
        => services
            .AddSingleton<ISerializer, SystemTextJsonSerializer>();
}