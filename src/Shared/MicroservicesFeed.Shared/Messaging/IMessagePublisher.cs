namespace MicroservicesFeed.Shared.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
        where T : class, IMessage;
}