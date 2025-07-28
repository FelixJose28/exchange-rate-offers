using ExchangeRateOffers.API2.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

[ApiController]
[Route("exchange")]
[Consumes("application/xml")]
[Produces("application/xml")]
public class ExchangeController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] Api2Request request)
    {
        Random random = new();
        var randomNumber = Math.Round((decimal)(random.NextDouble() * (double)request.Amount), 2);

        var response = new Api2Response { Result = randomNumber };
        return Ok(response);
    }
}
