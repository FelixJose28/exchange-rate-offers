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
            .Matches("^[A-Z]{3}$").WithMessage("Target currency must be in uppercase (e.g., DOP).");

        RuleFor(x => x.TargetCurrency)
            .NotEmpty().WithMessage("Target currency is required.")
            .Length(3).WithMessage("Target currency must be a 3-letter code.")
            .Matches("^[A-Z]{3}$").WithMessage("Target currency must be in uppercase (e.g., DOP).")
            //.NotEqual(x => x.SourceCurrency)
            //    .WithMessage("Source and target currencies must be different.")
            ;

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}