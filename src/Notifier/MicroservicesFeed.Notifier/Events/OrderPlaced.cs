using MicroservicesFeed.Shared.Messaging;

namespace MicroservicesFeed.Notifier.Events;

public record OrderPlaced(string OrderId, string Symbol) : IMessage;