using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Domain.Entities;
using System.Text.Json;

namespace ExchangeRateOffers.Api.Infrastructure.Services;

public class FawazCurrencyApiClient : IFawazCurrencyApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FawazCurrencyApiClient> _logger;

    public FawazCurrencyApiClient(HttpClient httpClient, ILogger<FawazCurrencyApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest exchangeRateRequest)
    {
        _logger.LogInformation($"Calling {nameof(FawazCurrencyApiClient)}");
        string fullUrl = $"{exchangeRateRequest.SourceCurrency.ToLower()}.json";
        using var response = await _httpClient.GetAsync(fullUrl);
        if (!response.IsSuccessStatusCode) return null;

        string content = await response.Content.ReadAsStringAsync();

        var json = JsonSerializer.Deserialize<JsonElement>(content);

        var sourceKey = exchangeRateRequest.SourceCurrency.ToLower();
        var targetKey = exchangeRateRequest.TargetCurrency.ToLower();

        if (json.TryGetProperty(sourceKey, out var ratesElement) &&
            ratesElement.TryGetProperty(targetKey, out var rateElement) &&
            rateElement.TryGetDecimal(out var rate))
        {
            decimal total = rate * exchangeRateRequest.Amount;
            ExchangeRateResponse exchangeRateResponse = new(nameof(FawazCurrencyApiClient), total);
            _logger.LogInformation("Successfully parsed exchange rate: {@ExchangeRateResponse}", exchangeRateResponse);
            return exchangeRateResponse;
        }

        return null;
    }
}
