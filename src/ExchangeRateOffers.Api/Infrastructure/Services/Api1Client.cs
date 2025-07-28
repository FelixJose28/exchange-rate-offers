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

    public Api1Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest  exchangeRateRequest)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, $"/exchange");

        var payload = new
        {
            from = exchangeRateRequest.SourceCurrency,
            to = exchangeRateRequest.TargetCurrency,
            value = exchangeRateRequest.Amount
        };
        string jsonPayload = JsonSerializer.Serialize(payload);
        request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request);
        var statusCode = response.StatusCode;
        var contentString = await response.Content.ReadAsStringAsync();


        if (!response.IsSuccessStatusCode) return null;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = JsonSerializer.Deserialize<Api1Response>(contentString, options);

        return result is null ? null : new ExchangeRateResponse("API1", result.Rate);
    }
}
