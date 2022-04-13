namespace MicroservicesFeed.Shared.Messaging;

public interface IMessageSubscriber
{
    Task SubscribeAsync<T>(string topic, Action<T> handler, CancellationToken cancellationToken = default)
        where T : class, IMessage;
}