namespace MicroservicesFeed.Shared.DateProvider;

public interface IDateProvider
{
    public DateTimeOffset Now { get; }
    public DateTimeOffset UtcNow { get; }
}