using MicroservicesFeed.Shared.Streaming;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesFeed.Shared.Redis.Streaming;

public static class RedisStreamingServiceCollectionExtensions
{
    public static IServiceCollection AddRedisStreaming(this IServiceCollection services)
        => services
            .AddSingleton<IStreamPublisher, RedisStreamPublisher>()
            .AddSingleton<IStreamSubscriber, RedisStreamSubscriber>();
}