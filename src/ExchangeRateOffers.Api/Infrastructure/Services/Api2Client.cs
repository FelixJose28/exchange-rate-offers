using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Domain.Entities;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace ExchangeRateOffers.Api.Infrastructure.Services;

public class Api2Client : IApi2Client
{
    private readonly HttpClient _httpClient;

    public Api2Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest exchangeRateRequest)
    {
        string fullUrl = $"https://cdn.jsdelivr.net/npm/@fawazahmed0/currency-api@latest/v1/currencies/{exchangeRateRequest.SourceCurrency.ToLower()}.json";
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
            return new ExchangeRateResponse("API2", total);
        }

        return null;
    }
}
