namespace ExchangeRateOffers.Api.Infrastructure.Dtos.Api3;

public class Api3Request
{
    public Api3RequestData Exchange { get; set; } = new();
}

public class Api3RequestData
{
    public string SourceCurrency { get; set; } = string.Empty;
    public string TargetCurrency { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
}
