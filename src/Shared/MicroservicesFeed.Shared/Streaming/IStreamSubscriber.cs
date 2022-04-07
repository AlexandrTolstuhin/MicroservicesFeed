namespace MicroservicesFeed.Shared.Streaming;

public interface IStreamSubscriber
{
    Task SubscribeAsync<T>(string topic, Action<T> handler, CancellationToken cancellationToken = default)
        where T : class;
}