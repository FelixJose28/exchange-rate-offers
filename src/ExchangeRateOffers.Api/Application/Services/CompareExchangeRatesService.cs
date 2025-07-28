using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Application.Interfaces.Services;
using ExchangeRateOffers.Api.Domain.Entities;

namespace ExchangeRateOffers.Api.Application.Services;

/// <summary>
/// Service that compares exchange rate offers from multiple external providers and returns the best one.
/// </summary>
public class CompareExchangeRatesService : ICompareExchangeRatesService
{
    private readonly IEnumerable<IExchangeRateProvider> _providers;
    private readonly ILogger<CompareExchangeRatesService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompareExchangeRatesService"/> class.
    /// </summary>
    /// <param name="providers">A collection of exchange rate providers.</param>
    /// <param name="logger">Logger used for logging information.</param>
    public CompareExchangeRatesService(IEnumerable<IExchangeRateProvider> providers, ILogger<CompareExchangeRatesService> logger)
    {
        _providers = providers;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves the best exchange rate offer by querying all registered providers.
    /// </summary>
    /// <param name="request">The exchange rate request including source currency, target currency, and amount.</param>
    /// <returns>
    /// The <see cref="ExchangeRateResponse"/> with the highest converted amount, or <c>null</c> if no valid responses are returned.
    /// </returns>
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
