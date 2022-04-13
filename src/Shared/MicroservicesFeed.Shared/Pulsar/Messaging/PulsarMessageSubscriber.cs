using System.Reflection;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using MicroservicesFeed.Shared.Messaging;
using MicroservicesFeed.Shared.Serialization;
using Microsoft.Extensions.Logging;
using IMessage = MicroservicesFeed.Shared.Messaging.IMessage;

namespace MicroservicesFeed.Shared.Pulsar.Messaging;

internal class PulsarMessageSubscriber : IMessageSubscriber
{
    private readonly IPulsarClient _pulsarClient;
    private readonly ISerializer _serializer;
    private readonly ILogger<PulsarMessageSubscriber> _logger;
    private readonly string _consumerName;

    public PulsarMessageSubscriber(
        IPulsarClient pulsarClient,
        ISerializer serializer,
        ILogger<PulsarMessageSubscriber> logger)
    {
        _pulsarClient = pulsarClient;
        _serializer = serializer;
        _logger = logger;

        _consumerName = Assembly.GetEntryAssembly()?.FullName?.Split(",")[0].ToLowerInvariant() ?? string.Empty;
    }

    public async Task SubscribeAsync<T>(string topic, Action<T> handler, CancellationToken cancellationToken = default)
        where T : class, IMessage
    {
        var subscription = $"{_consumerName}_{topic}";
        var consumer = _pulsarClient.NewConsumer()
            .SubscriptionName(subscription)
            .Topic($"persistent://public/default/{topic}")
            .Create();

        await foreach (var message in consumer.Messages(cancellationToken))
        {
            var producer = message.Properties["producer"];
            var customId = message.Properties["custom_id"];

            _logger.LogInformation(
                "Received a message with ID: '{MessageId}' from: '{Producer}' with custom ID: '{CustomId}'",
                message.MessageId,
                producer,
                customId);

            var payload = _serializer.DeserializeBytes<T>(message.Data.FirstSpan.ToArray());
            if (payload is not null)
            {
                handler(payload);
            }

            await consumer.Acknowledge(message, cancellationToken);
        }
    }
}