using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Domain.Entities;
using System.Text.Json;

namespace ExchangeRateOffers.Api.Infrastructure.Services;

public class Api1Client : IApi1Client
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<Api1Client> _logger;

    public Api1Client(HttpClient httpClient, ILogger<Api1Client> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest exchangeRateRequest)
    {
        _logger.LogInformation($"Calling {nameof(Api1Client)}");
        string baseUrl = "https://open.er-api.com/v6/latest";
        string fullUrl = $"{baseUrl}/{exchangeRateRequest.SourceCurrency.ToUpper()}";
        using var response = await _httpClient.GetAsync(fullUrl);
        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(content);

        if (json.TryGetProperty("rates", out var ratesElement) &&
            ratesElement.TryGetProperty(exchangeRateRequest.TargetCurrency.ToUpper(), out var rateElement) &&
            rateElement.TryGetDecimal(out var rate))
        {
            decimal total = rate * exchangeRateRequest.Amount;
            return new ExchangeRateResponse(nameof(Api1Client), total);
        }

        return null;
    }
}
