using ExchangeRateOffers.Api.Domain.Entities;

namespace ExchangeRateOffers.Api.Application.Interfaces.Services;

public interface ICompareExchangeRatesService
{
    Task<ExchangeRateResponse?> GetBestRateAsync(ExchangeRateRequest request);
}
