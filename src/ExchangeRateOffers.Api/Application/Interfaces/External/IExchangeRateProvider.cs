using ExchangeRateOffers.Api.Domain.Entities;

namespace ExchangeRateOffers.Api.Application.Interfaces.External;

public interface IExchangeRateProvider
{
    Task<ExchangeRateResponse?> GetExchangeRateAsync(ExchangeRateRequest request);
}
