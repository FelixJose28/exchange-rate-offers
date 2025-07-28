using ExchangeRateOffers.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

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
        // Arrange: Invalid currency format (lowercase, not 3 chars)
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
        // Arrange: Use improbable currency to trigger empty result (depends on live APIs)
        var request = new ExchangeRateRequest("XXX", "YYY", 100);

        // Act
        var response = await _client.PostAsJsonAsync("/exchange-rate/compare", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Equal("No offers found", problemDetails.Title);
    }
}
