using System.Threading.Channels;
using MicroservicesFeed.Quotes.Pricing.Requests;

namespace MicroservicesFeed.Quotes.Pricing.Services;

internal sealed class PricingRequestChannel
{
    public readonly Channel<IPricingRequest> Requests = Channel.CreateUnbounded<IPricingRequest>();
}