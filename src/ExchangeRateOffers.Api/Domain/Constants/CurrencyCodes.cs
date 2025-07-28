namespace ExchangeRateOffers.Api.Domain.Constants;

public static class CurrencyCodes
{
    public static readonly HashSet<string> ValidIsoCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "AFN", "ALL", "DZD", "USD", "EUR", "AOA", "XCD", "ARS", "AMD", "AWG",
        "AUD", "AZN", "BSD", "BHD", "BDT", "BBD", "BYN", "BZD", "XOF", "BMD",
        "BTN", "INR", "BOB", "BOV", "BAM", "BWP", "NOK", "BRL", "BND", "BGN",
        "BIF", "CVE", "KHR", "XAF", "CAD", "KYD", "CLF", "CLP", "CNY", "COP",
        "COU", "KMF", "CDF", "NZD", "CRC", "CUC", "CUP", "CZK", "DJF", "DOP",
        "EGP", "SVC", "ERN", "ETB", "FKP", "FJD", "GMD", "GEL", "GHS", "GIP",
        "GTQ", "GNF", "GYD", "HTG", "HNL", "HKD", "HUF", "ISK", "IDR", "XDR",
        "IRR", "IQD", "ILS", "JMD", "JPY", "JOD", "KZT", "KES", "KPW", "KRW",
        "KWD", "KGS", "LAK", "LBP", "LSL", "LRD", "LYD", "CHF", "MOP", "MGA",
        "MWK", "MYR", "MVR", "MRU", "MUR", "MXN", "MXV", "MDL", "MNT", "MAD",
        "MZN", "MMK", "NAD", "NPR", "ANG", "TWD", "NIO", "NGN", "OMR", "PKR",
        "PAB", "PGK", "PYG", "PEN", "PHP", "PLN", "QAR", "RON", "RUB", "RWF",
        "SHP", "SAR", "RSD", "SCR", "SLL", "SGD", "XSU", "SBD", "SOS", "ZAR",
        "SSP", "LKR", "SDG", "SRD", "SZL", "SEK", "CHE", "CHW", "SYP", "THB",
        "TOP", "TTD", "TND", "TRY", "TMT", "UGX", "UAH", "AED", "GBP", "USN",
        "UYU", "UZS", "VUV", "VES", "VED", "VND", "XPF", "YER", "ZMW", "ZWL"
    };
}
