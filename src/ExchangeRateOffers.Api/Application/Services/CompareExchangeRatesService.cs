using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Application.Interfaces.Services;
using ExchangeRateOffers.Api.Domain.Entities;

namespace ExchangeRateOffers.Api.Application.Services;

public class CompareExchangeRatesService : ICompareExchangeRatesService
{
    private readonly IEnumerable<IExchangeRateProvider> _providers;
    private readonly ILogger<CompareExchangeRatesService> _logger;

    public CompareExchangeRatesService(IEnumerable<IExchangeRateProvider> providers, ILogger<CompareExchangeRatesService> logger)
    {
        _providers = providers;
        _logger = logger;
    }

    public async Task<ExchangeRateResponse?> GetBestRateAsync(ExchangeRateRequest request)
    {
        _logger.LogInformation($"Calling the apis");
        var tasks = _providers.Select(p => p.GetExchangeRateAsync(request));
        var results = await Task.WhenAll(tasks);


        _logger.LogInformation($"Get the values from the apis");

        return results
            .Where(r => r is not null)
            .OrderByDescending(r => r!.ConvertedAmount)
            .FirstOrDefault();
    }
}
