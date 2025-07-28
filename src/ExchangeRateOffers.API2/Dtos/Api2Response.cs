using System.Xml.Serialization;

namespace ExchangeRateOffers.API2.Dtos;

[XmlRoot("ExchangeResponse")]


public record ExchangeResponse
{
    public decimal Result { get; set; }
}
