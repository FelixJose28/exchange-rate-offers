using ExchangeRateOffers.API3.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExchangeRateOffers.API3.Controllers;

[ApiController]
[Route("exchange")]
public class ExchangeController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] Api3Request request)
    {

        Random random = new();
        var randomNumber = Math.Round((decimal)(random.NextDouble() * (double)request.Exchange.Quantity), 2);

        Api3Response result = new(
            StatusCode: Convert.ToInt32(HttpStatusCode.OK),
            Message: "Success",
            Data: new Api3Data(randomNumber)
        );
        return Ok(result);
    }
}

