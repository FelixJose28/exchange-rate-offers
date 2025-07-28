using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Application.Interfaces.Services;
using ExchangeRateOffers.Api.Domain.Entities;

namespace ExchangeRateOffers.Api.Application.Services;

public class CompareExchangeRatesService: ICompareExchangeRatesService
{
    private readonly IApi1Client _api1Client;
    private readonly IApi2Client _api2Client;
    private readonly IApi3Client _api3Client;

    public CompareExchangeRatesService(
        IApi1Client api1Client,
        IApi2Client api2Client,
        IApi3Client api3Client)
    {
        _api1Client = api1Client;
        _api2Client = api2Client;
        _api3Client = api3Client;
    }

    public async Task<ExchangeRateResponse?> GetBestRateAsync(ExchangeRateRequest request)
    {
        var tasks = new List<Task<ExchangeRateResponse?>>
        {
            _api1Client.GetExchangeRateAsync(request),
            _api2Client.GetExchangeRateAsync(request),
            _api3Client.GetExchangeRateAsync(request)
        };

        var results = await Task.WhenAll(tasks);

        return results
            .Where(r => r is not null)
            .OrderByDescending(r => r!.ConvertedAmount)
            .FirstOrDefault();
    }
}
