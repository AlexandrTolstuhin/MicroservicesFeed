using MicroservicesFeed.Shared.Serialization;
using MicroservicesFeed.Shared.Streaming;
using StackExchange.Redis;

namespace MicroservicesFeed.Shared.Redis.Streaming;

internal sealed class RedisStreamPublisher : IStreamPublisher
{
    private readonly ISerializer _serializer;
    private readonly ISubscriber _subscriber;

    public RedisStreamPublisher(IConnectionMultiplexer connectionMultiplexer, ISerializer serializer)
    {
        _serializer = serializer;
        _subscriber = connectionMultiplexer.GetSubscriber();
    }

    public Task PublishAsync<T>(string topic, T data, CancellationToken cancellationToken = default) where T : class
    {
        var payload = _serializer.Serialize(data);
        return _subscriber.PublishAsync(topic, payload);
    }
}