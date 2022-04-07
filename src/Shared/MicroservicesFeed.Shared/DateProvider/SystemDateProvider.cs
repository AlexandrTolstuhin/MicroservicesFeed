namespace MicroservicesFeed.Shared.DateProvider;

internal class SystemDateProvider : IDateProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}