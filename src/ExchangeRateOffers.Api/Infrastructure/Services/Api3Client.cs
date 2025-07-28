using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Domain.Entities;
using ExchangeRateOffers.Api.Infrastructure.Dtos.Api3;
using System.Text.Json;
using System.Text;

namespace ExchangeRateOffers.Api.Infrastructure.Services;

public class Api3Client : IApi3Client
{
    private readonly HttpClient _httpClient;

    public Api3Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest request)
    {
        var payload = new Api3Request
        {
            Exchange = new Api3RequestData
            {
                SourceCurrency = request.SourceCurrency,
                TargetCurrency = request.TargetCurrency,
                Quantity = request.Amount
            }
        };

        string json = JsonSerializer.Serialize(payload);

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/exchange")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        using var response = await _httpClient.SendAsync(httpRequest);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = JsonSerializer.Deserialize<Api3Response>(content, options);

        return result == null ? null : new ExchangeRateResponse("API3",result.Data.Total);
    }
}