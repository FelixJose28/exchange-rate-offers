using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Domain.Entities;
using System.Text.Json;

namespace ExchangeRateOffers.Api.Infrastructure.Services;

public class ErApiClient : IErApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ErApiClient> _logger;

    public ErApiClient(HttpClient httpClient, ILogger<ErApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest exchangeRateRequest)
    {
        _logger.LogInformation($"Calling {nameof(ErApiClient)}");
        string fullUrl = $"{exchangeRateRequest.SourceCurrency.ToUpper()}";
        using var response = await _httpClient.GetAsync(fullUrl);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;

        var json = JsonSerializer.Deserialize<JsonElement>(content);

        if (json.TryGetProperty("rates", out var ratesElement) &&
            ratesElement.TryGetProperty(exchangeRateRequest.TargetCurrency.ToUpper(), out var rateElement) &&
            rateElement.TryGetDecimal(out var rate))
        {
            decimal total = rate * exchangeRateRequest.Amount;
            ExchangeRateResponse exchangeRateResponse = new(nameof(ErApiClient), total);
            _logger.LogInformation("Successfully parsed exchange rate: {@ExchangeRateResponse}", exchangeRateResponse);
            return exchangeRateResponse;
        }

        return null;
    }
}
