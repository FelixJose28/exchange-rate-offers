namespace ExchangeRateOffers.API3.Dtos;

public record Api3Request(ExchangeData Exchange);

public record ExchangeData
(
    string SourceCurrency, 
    string TargetCurrency, 
    decimal Quantity 
);
