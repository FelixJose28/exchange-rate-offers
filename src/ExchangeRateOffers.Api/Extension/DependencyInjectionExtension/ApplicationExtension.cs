using ExchangeRateOffers.Api.Application.Interfaces.External;
using ExchangeRateOffers.Api.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;

namespace ExchangeRateOffers.Api.Extension.DependencyInjectionExtension;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}