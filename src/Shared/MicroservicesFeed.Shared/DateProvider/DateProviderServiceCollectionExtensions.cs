using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesFeed.Shared.DateProvider;

public static class DateProviderServiceCollectionExtensions
{
    public static IServiceCollection AddDateProvider(this IServiceCollection services)
        => services
            .AddSingleton<IDateProvider, SystemDateProvider>();
}