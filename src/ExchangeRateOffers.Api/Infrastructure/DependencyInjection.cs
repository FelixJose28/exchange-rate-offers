using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Infrastructure.Services;
using System.Net.Http.Headers;

namespace ExchangeRateOffers.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IErApiClient, ErApiClient>(client =>
        {
            string api1Url = GetBaseUrl(configuration, "ExternalApis:Api1Url");
            client.BaseAddress = new Uri(api1Url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        services.AddScoped<IExchangeRateProvider>(sp => sp.GetRequiredService<IErApiClient>());

        services.AddHttpClient<IFawazCurrencyApiClient, FawazCurrencyApiClient>(client =>
        {
            string api2Url = GetBaseUrl(configuration, "ExternalApis:Api2Url");
            client.BaseAddress = new Uri(api2Url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        services.AddScoped<IExchangeRateProvider>(sp => sp.GetRequiredService<IFawazCurrencyApiClient>());

        services.AddHttpClient<IFrankfurterApiClient, FrankfurterApiClient>(client =>
        {
            string api3Url = GetBaseUrl(configuration, "ExternalApis:Api3Url");
            client.BaseAddress = new Uri(api3Url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        services.AddScoped<IExchangeRateProvider>(sp => sp.GetRequiredService<IFrankfurterApiClient>());

        return services;
    }
    private static string GetBaseUrl(IConfiguration configuration, string key)
    {
        return configuration[key] ?? throw new InvalidOperationException($"API base URL is not configured.");
    }
}
