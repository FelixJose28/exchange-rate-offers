using ExchangeRateOffers.Api.Application.Interfaces.Services;
using ExchangeRateOffers.Api.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExchangeRateOffers.Api.Controllers;

/// <summary>
/// Handles exchange rate comparison requests.
/// </summary>
[ApiController]
[Route("exchange-rate")]
public class ExchangeRateController : ControllerBase
{

    private readonly ILogger<ExchangeRateController> _logger;
    private readonly IValidator<ExchangeRateRequest> _validator;
    private readonly ICompareExchangeRatesService _compareService;


    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeRateController"/> class.
    /// </summary>
    /// <param name="logger">Logger for logging information and errors.</param>
    /// <param name="validator">Validator for validating incoming requests.</param>
    /// <param name="compareService">Service to compare exchange rate offers from different providers.</param>
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

    /// <summary>
    /// Compares exchange rate offers from multiple external APIs and returns the best one.
    /// </summary>
    /// <param name="request">The exchange rate request containing source currency, target currency, and amount.</param>
    /// <returns>
    /// Returns the best exchange rate offer found. 
    /// Returns 400 if validation fails, or 404 if no valid offer is available.
    /// </returns>
    /// <response code="200">Returns the best exchange rate offer.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If no valid exchange rate offers are available.</response>
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
