using System.Buffers;
using System.Collections.Concurrent;
using System.Reflection;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using MicroservicesFeed.Shared.Messaging;
using MicroservicesFeed.Shared.Serialization;
using Microsoft.Extensions.Logging;
using IMessage = MicroservicesFeed.Shared.Messaging.IMessage;

namespace MicroservicesFeed.Shared.Pulsar.Messaging;

internal class PulsarMessagePublisher : IMessagePublisher
{
    private readonly ConcurrentDictionary<string, IProducer<ReadOnlySequence<byte>>> _producers = new();

    private readonly ISerializer _serializer;
    private readonly IPulsarClient _pulsarClient;
    private readonly ILogger<PulsarMessagePublisher> _logger;
    private readonly string _producerName;

    public PulsarMessagePublisher(
        ISerializer serializer,
        IPulsarClient pulsarClient,
        ILogger<PulsarMessagePublisher> logger)
    {
        _serializer = serializer;
        _pulsarClient = pulsarClient;
        _logger = logger;

        _producerName = Assembly.GetEntryAssembly()?.FullName?.Split(",")[0].ToLowerInvariant() ?? string.Empty;
    }

    public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken)
        where T : class, IMessage
    {
        var producer = _producers.GetOrAdd(
            topic,
            _pulsarClient.NewProducer()
                .ProducerName(_producerName)
                .Topic($"persistent://public/default/{topic}")
                .Create());

        var payload = _serializer.SerializeBytes(message);
        MessageMetadata metadata = new()
        {
            ["custom_id"] = Guid.NewGuid().ToString("N"),
            ["producer"] = _producerName
        };
        var messageId = await producer.Send(metadata, payload, cancellationToken);

        _logger.LogInformation("Sent a message with ID: '{MessageId}'", messageId);
    }
}