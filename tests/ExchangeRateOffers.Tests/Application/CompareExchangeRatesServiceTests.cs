using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Application.Services;
using ExchangeRateOffers.Api.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateOffers.Tests.Application;

public class CompareExchangeRatesServiceTests
{
    private readonly Mock<ILogger<CompareExchangeRatesService>> _loggerMock = new();

    [Fact]
    public async Task GetBestRateAsync_ReturnsHighestRate()
    {
        // Arrange
        var request = new ExchangeRateRequest("USD", "EUR", 100);

        var mock1 = new Mock<IExchangeRateProvider>();
        mock1.Setup(x => x.GetExchangeRateAsync(request))
             .ReturnsAsync(new ExchangeRateResponse("API1", 90));

        var mock2 = new Mock<IExchangeRateProvider>();
        mock2.Setup(x => x.GetExchangeRateAsync(request))
             .ReturnsAsync(new ExchangeRateResponse("API2", 95));

        var mock3 = new Mock<IExchangeRateProvider>();
        mock3.Setup(x => x.GetExchangeRateAsync(request))
             .ReturnsAsync(new ExchangeRateResponse("API3", 93));

        var service = new CompareExchangeRatesService(
            new[] { mock1.Object, mock2.Object, mock3.Object },
            _loggerMock.Object
        );

        // Act
        var result = await service.GetBestRateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("API2", result.Provider);
        Assert.Equal(95, result.ConvertedAmount);
    }

    [Fact]
    public async Task GetBestRateAsync_AllProvidersReturnNull_ReturnsNull()
    {
        // Arrange
        var request = new ExchangeRateRequest("USD", "EUR", 100);

        var mock1 = new Mock<IExchangeRateProvider>();
        mock1.Setup(x => x.GetExchangeRateAsync(request))
             .ReturnsAsync((ExchangeRateResponse?)null);

        var mock2 = new Mock<IExchangeRateProvider>();
        mock2.Setup(x => x.GetExchangeRateAsync(request))
             .ReturnsAsync((ExchangeRateResponse?)null);

        var service = new CompareExchangeRatesService(
            new[] { mock1.Object, mock2.Object },
            _loggerMock.Object
        );

        // Act
        var result = await service.GetBestRateAsync(request);

        // Assert
        Assert.Null(result);
    }
}
