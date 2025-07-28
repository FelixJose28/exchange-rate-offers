using System.Xml.Serialization;

namespace ExchangeRateOffers.API2.Dtos;

[XmlRoot("ExchangeResponse")]


public record Api2Response
{
    public decimal Result { get; set; }
}
