using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Domain.Entities;
using System.Text.Json;

namespace ExchangeRateOffers.Api.Infrastructure.Services;

public class Api3Client : IApi3Client
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<Api2Client> _logger;

    public Api3Client(HttpClient httpClient, ILogger<Api2Client> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest exchangeRateRequest)
    {
        _logger.LogInformation($"Calling {nameof(Api3Client)}");
        var source = exchangeRateRequest.SourceCurrency.ToUpper();
        var target = exchangeRateRequest.TargetCurrency.ToUpper();
        var amount = exchangeRateRequest.Amount;

        string url = $"https://api.frankfurter.app/latest?amount={amount}&from={source}&to={target}";

        using var response = await _httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) return null;

        var json = JsonSerializer.Deserialize<JsonElement>(content);

        if (json.TryGetProperty("rates", out var ratesElement) &&
            ratesElement.TryGetProperty(target, out var rateElement) &&
            rateElement.TryGetDecimal(out var total))
        {
            return new ExchangeRateResponse(nameof(Api3Client), total);
        }

        return null;
    }
}