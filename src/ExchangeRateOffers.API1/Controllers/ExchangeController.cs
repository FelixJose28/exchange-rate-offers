using ExchangeRateOffers.API1.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateOffers.API1.Controllers;

[ApiController]
[Route("exchange")]
public class ExchangeController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] Api1Request request)
    {

        Random random = new();
        decimal randomNumber = Math.Round((decimal)(random.NextDouble() * (double)request.Value), 2);

        return Ok(new Api1Response(randomNumber));
    }
}

