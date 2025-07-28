namespace ExchangeRateOffers.Api.Infrastructure.Dtos.Api3;

public record Api3Response
(
    int StatusCode,
    string Message,
    Api3Data Data
);

public record Api3Data(decimal Total);
