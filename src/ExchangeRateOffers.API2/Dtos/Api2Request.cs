using System.Xml.Serialization;

namespace ExchangeRateOffers.API2.Dtos;

[XmlRoot("ExchangeRequest")]
public record Api2Request
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
