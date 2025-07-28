using ExchangeRateOffers.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace ExchangeRateOffers.Tests.Controllers;

public class ExchangeRateControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ExchangeRateControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Compare_ReturnsOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new ExchangeRateRequest("USD", "EUR", 100);

        // Act
        var response = await _client.PostAsJsonAsync("/exchange-rate/compare", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Provider));
        Assert.True(result.ConvertedAmount > 0);
    }

    [Fact]
    public async Task Compare_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new ExchangeRateRequest("us", "eur", -5);

        // Act
        var response = await _client.PostAsJsonAsync("/exchange-rate/compare", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Equal("Validation failed.", problemDetails.Title);
    }

    [Fact]
    public async Task Compare_ReturnsNotFound_WhenAllProvidersFail()
    {
        // Arrange
        var request = new ExchangeRateRequest("XXX", "YYY", 100);

        // Act
        var response = await _client.PostAsJsonAsync("/exchange-rate/compare", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Equal("Validation failed.", problemDetails.Title);
        Assert.Contains("SourceCurrency", problemDetails.Errors.Keys);
        Assert.Contains("TargetCurrency", problemDetails.Errors.Keys);
        Assert.Contains("Source currency code is not valid.", problemDetails.Errors["SourceCurrency"]);
        Assert.Contains("Target currency code is not valid.", problemDetails.Errors["TargetCurrency"]);
    }
}
