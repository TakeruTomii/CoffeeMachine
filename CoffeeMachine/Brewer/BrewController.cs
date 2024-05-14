using CoffeeMachine.Brewer.Interface;
using CoffeeMachine.Brewer.Model;
using CoffeeMachine.CustomException;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Brewer
{
    [ApiController]
    public class BrewController : ControllerBase
    {
        private readonly ILogger<BrewController> _logger;
        private readonly IBrewService _brewService;

        public BrewController(
            ILogger<BrewController> logger,
            IBrewService brewService)
        {
            _logger = logger;
            _brewService = brewService;
        }

        [HttpGet]
        [Route("brew-coffee")]
        public async Task<ActionResult<Coffee>> BrewCoffeeAsync()
        {
            try
            {
                return await _brewService.Brew();
            }
            catch (TeaPotException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return StatusCode(StatusCodes.Status418ImATeapot, value: ex.Message);
            }
            catch (OutOfCoffeeException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, value: ex.Message);
            }
        }
    }
}
