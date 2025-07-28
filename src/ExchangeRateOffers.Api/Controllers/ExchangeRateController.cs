using ExchangeRateOffers.Api.Application.Interfaces.Services;
using ExchangeRateOffers.Api.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExchangeRateOffers.Api.Controllers;

[ApiController]
[Route("exchange-rate")]
public class ExchangeRateController : ControllerBase
{

    private readonly ILogger<ExchangeRateController> _logger;
    private readonly IValidator<ExchangeRateRequest> _validator;
    private readonly ICompareExchangeRatesService _compareService;


    public ExchangeRateController
    (
        ILogger<ExchangeRateController> logger,
        IValidator<ExchangeRateRequest> validator,
        ICompareExchangeRatesService compareService
    )
    {
        _logger = logger;
        _validator = validator;
        _compareService = compareService;
    }

    [HttpPost("compare")]
    public async Task<IActionResult> Compare([FromBody] ExchangeRateRequest request)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogError
            (
                "[{Time}] Validation failed, occurs an error in {@Location}: {@Errors}. HttpStatusCode: {@HttpStatusCode}. Request: {@Request}",
                DateTime.Now,
                $"Controller {nameof(ExchangeRateController)} method {nameof(Compare)}",
                validationResult.Errors,
                HttpStatusCode.BadRequest,
                request
            );

            HttpValidationProblemDetails problemDetails = new(validationResult.ToDictionary())
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Validation failed.",
                Detail = "One or more validation errors ocurred."
            };

            return BadRequest(problemDetails);
        }

        ExchangeRateResponse? bestOffer = await _compareService.GetBestRateAsync(request);

        if (bestOffer is null)
        {
            _logger.LogWarning
             (
                "[{Time}] No valid exchange rate offers are available at the moment in {@Location}: {@Errors}. HttpStatusCode: {@HttpStatusCode}. Request: {@Request}",
                DateTime.Now,
                $"Controller {nameof(ExchangeRateController)} method {nameof(Compare)}",
                validationResult.Errors,
                HttpStatusCode.BadRequest,
                request
             );

            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "No offers found",
                Detail = "No valid exchange rate offers are available at the moment."
            };

            return NotFound(problemDetails);
        }

        return Ok(bestOffer);
    }
}
