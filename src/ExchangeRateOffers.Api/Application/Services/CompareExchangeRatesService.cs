using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Application.Interfaces.Services;
using ExchangeRateOffers.Api.Domain.Entities;

namespace ExchangeRateOffers.Api.Application.Services;

public class CompareExchangeRatesService : ICompareExchangeRatesService
{
    private readonly IApi1Client _api1Client;
    private readonly IApi2Client _api2Client;
    private readonly IApi3Client _api3Client;
    private readonly ILogger<CompareExchangeRatesService> _logger;

    public CompareExchangeRatesService
    (
        IApi1Client api1Client,
        IApi2Client api2Client,
        IApi3Client api3Client,
        ILogger<CompareExchangeRatesService> logger
    )
    {
        _api1Client = api1Client;
        _api2Client = api2Client;
        _api3Client = api3Client;
        _logger = logger;
    }

    public async Task<ExchangeRateResponse?> GetBestRateAsync(ExchangeRateRequest request)
    {
        _logger.LogInformation($"Calling the apis");
        var tasks = new List<Task<ExchangeRateResponse?>>
        {
            _api1Client.GetExchangeRateAsync(request),
            _api2Client.GetExchangeRateAsync(request),
            _api3Client.GetExchangeRateAsync(request)
        };

        _logger.LogInformation($"Get the values from the apis");
        var results = await Task.WhenAll(tasks);
        return results
            .Where(r => r is not null)
            .OrderByDescending(r => r!.ConvertedAmount)
            .FirstOrDefault();
    }
}
