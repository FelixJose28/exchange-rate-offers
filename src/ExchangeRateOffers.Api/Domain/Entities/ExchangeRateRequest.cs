namespace ExchangeRateOffers.Api.Domain.Entities;

public record ExchangeRateRequest(string SourceCurrency, string TargetCurrency, decimal Amount);
