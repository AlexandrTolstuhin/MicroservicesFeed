using MicroservicesFeed.Quotes.Pricing.Requests;
using MicroservicesFeed.Shared.Streaming;

namespace MicroservicesFeed.Quotes.Pricing.Services;

internal class PricingBackgroundService : BackgroundService
{
    private readonly IPricingService _pricingService;
    private readonly IStreamPublisher _streamPublisher;
    private readonly PricingRequestChannel _pricingRequestChannel;
    private readonly ILogger<PricingBackgroundService> _logger;

    private volatile int _runningStatus;
    private CancellationTokenSource _tokenSource = new();

    public PricingBackgroundService(
        IPricingService pricingService,
        IStreamPublisher streamPublisher,
        PricingRequestChannel pricingRequestChannel,
        ILogger<PricingBackgroundService> logger)
    {
        _pricingService = pricingService;
        _streamPublisher = streamPublisher;
        _pricingRequestChannel = pricingRequestChannel;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var request in _pricingRequestChannel.Requests.Reader.ReadAllAsync(stoppingToken))
        {
            _logger.LogInformation(
                "Pricing background service has received the request: '{RequestName}'",
                request.GetType().Name);

            _ = request switch
            {
                StartPricing => StartGeneratorAsync(),
                StopPricing => StopGeneratorAsync(),
                _ => Task.CompletedTask
            };
        }
    }

    private async Task StartGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
        {
            _logger.LogInformation("Pricing generator is already running");
            return;
        }

        _tokenSource = new();

        try
        {
            await foreach (var currencyPair in _pricingService.GetPrices(_tokenSource.Token))
            {
                await _streamPublisher.PublishAsync("pricing", currencyPair);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error when getting prices from the service");
            Console.WriteLine(exception);
        }
    }

    private Task StopGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
        {
            _logger.LogInformation("Pricing generator is not running");
            return Task.CompletedTask;
        }

        _tokenSource.Cancel();
        return Task.CompletedTask;
    }
}