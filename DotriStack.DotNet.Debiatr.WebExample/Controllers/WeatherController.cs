using DotriStack.DotNet.Debiatr.WebExample.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace DotriStack.DotNet.Debiatr.WebExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetWeather()
        {
            var result = await mediator.Send(new GetWeatherQuery());
            return Ok(result);
        }
    }
} 