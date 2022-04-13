using MicroservicesFeed.Shared.Messaging;

namespace MicroservicesFeed.Aggregator.Events;

internal record OrderPlaced(string OrderId, string Symbol) : IMessage;