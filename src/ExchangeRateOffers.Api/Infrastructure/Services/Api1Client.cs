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
            return new ExchangeRateResponse(nameof(Api1Client), total);
        }

        return null;
    }
}
