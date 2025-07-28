using System.Net.Http;
using System.Text.Json;
using System.Text;
using ExchangeRateOffers.Api.Domain.Entities;
using ExchangeRateOffers.Api.Application.Interfaces.External;
using Microsoft.AspNetCore.Http;
using ExchangeRateOffers.Api.Infrastructure.Dtos.Api1;

namespace ExchangeRateOffers.Api.Infrastructure.Services;

public class Api1Client: IApi1Client
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<Api1Client> _logger;
    private const string BaseUrl = "https://open.er-api.com/v6/latest";

    public Api1Client(HttpClient httpClient, ILogger<Api1Client> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest  exchangeRateRequest)
    {
        _logger.LogInformation($"Calling {nameof(Api1Client)}");
        string fullUrl = $"{BaseUrl}/{exchangeRateRequest.SourceCurrency.ToUpper()}";
        using HttpRequestMessage request = new(HttpMethod.Get, fullUrl);
        using var response = await _httpClient.SendAsync(request);
        var statusCode = response.StatusCode;
        var contentString = await response.Content.ReadAsStringAsync();


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
