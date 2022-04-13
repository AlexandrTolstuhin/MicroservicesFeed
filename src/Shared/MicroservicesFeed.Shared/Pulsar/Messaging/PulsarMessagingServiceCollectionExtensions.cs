using MicroservicesFeed.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesFeed.Shared.Pulsar.Messaging;

public static class PulsarMessagingServiceCollectionExtensions
{
    public static IServiceCollection AddPulsarMessaging(this IServiceCollection services)
        => services
            .AddSingleton<IMessagePublisher, PulsarMessagePublisher>()
            .AddSingleton<IMessageSubscriber, PulsarMessageSubscriber>();
}