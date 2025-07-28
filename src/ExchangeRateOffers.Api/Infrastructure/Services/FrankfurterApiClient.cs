using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Domain.Entities;
using System.Text.Json;

namespace ExchangeRateOffers.Api.Infrastructure.Services;

public class FrankfurterApiClient : IFrankfurterApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FrankfurterApiClient> _logger;

    public FrankfurterApiClient(HttpClient httpClient, ILogger<FrankfurterApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest exchangeRateRequest)
    {
        _logger.LogInformation($"Calling {nameof(FrankfurterApiClient)}");
        var source = exchangeRateRequest.SourceCurrency.ToUpper();
        var target = exchangeRateRequest.TargetCurrency.ToUpper();
        var amount = exchangeRateRequest.Amount;

        string url = $"/latest?amount={amount}&from={source}&to={target}";
        using var response = await _httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) return null;

        var json = JsonSerializer.Deserialize<JsonElement>(content);

        if (json.TryGetProperty("rates", out var ratesElement) &&
            ratesElement.TryGetProperty(target, out var rateElement) &&
            rateElement.TryGetDecimal(out var total))
        {
            ExchangeRateResponse exchangeRateResponse = new(nameof(FrankfurterApiClient), total);
            _logger.LogInformation("Successfully parsed exchange rate: {@ExchangeRateResponse}", exchangeRateResponse);
            return exchangeRateResponse;
        }

        return null;
    }
}