using ExchangeRateOffers.Api.Domain.Constants;
using ExchangeRateOffers.Api.Domain.Entities;
using FluentValidation;

namespace ExchangeRateOffers.Api.Application.Validators;

public class ExchangeRateRequestValidator : AbstractValidator<ExchangeRateRequest>
{
    public ExchangeRateRequestValidator()
    {
        RuleFor(x => x.SourceCurrency)
            .NotEmpty().WithMessage("Source currency is required.")
            .Length(3).WithMessage("Source currency must be a 3-letter code.")
            .Must(code => CurrencyCodes.ValidIsoCodes.Contains(code))
            .WithMessage("Source currency code is not valid."); ;

        RuleFor(x => x.TargetCurrency)
            .NotEmpty().WithMessage("Target currency is required.")
            .Length(3).WithMessage("Target currency must be a 3-letter code.")
            .Must(code => CurrencyCodes.ValidIsoCodes.Contains(code))
            .WithMessage("Target currency code is not valid.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}