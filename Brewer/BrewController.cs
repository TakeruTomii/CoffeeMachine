using CoffeeMachine.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Brewer
{
    [Route("brew-coffee")]
    [ApiController]
    public class BrewController : ControllerBase
    {
        private readonly ILogger<BrewController> _logger;

        public BrewController(ILogger<BrewController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("brew-coffee")]
        public Coffee BrewCoffee()
        {
            try
            {
                return ;
            } 
            catch (Exception ex)
            {

            }

        }
    }
}
