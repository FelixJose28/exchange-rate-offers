using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Domain.Entities;
using System.Text;
using System.Xml;

namespace ExchangeRateOffers.Api.Infrastructure.Services;

public class Api2Client : IApi2Client
{
    private readonly HttpClient _httpClient;

    public Api2Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest request)
    {
        string xmlPayload = $@"
        <ExchangeRequest>
          <From>{request.SourceCurrency}</From>
          <To>{request.TargetCurrency}</To>
          <Amount>{request.Amount}</Amount>
        </ExchangeRequest>";

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/exchange")
        {
            Content = new StringContent(xmlPayload, Encoding.UTF8, "application/xml")
        };

        using var response = await _httpClient.SendAsync(httpRequest);
        if (!response.IsSuccessStatusCode) return null;

        string content = await response.Content.ReadAsStringAsync();
        var xml = new XmlDocument();
        xml.LoadXml(content);
        var resultNode = xml.SelectSingleNode("//Result");

        return decimal.TryParse(resultNode?.InnerText, out decimal result)
            ? new ExchangeRateResponse("API2", result)
            : null;
    }
}
