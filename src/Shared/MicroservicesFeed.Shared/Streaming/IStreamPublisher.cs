namespace MicroservicesFeed.Shared.Streaming;

public interface IStreamPublisher
{
    Task PublishAsync<T>(string topic, T data, CancellationToken cancellationToken = default) where T : class;
}