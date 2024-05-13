using CoffeeMachine.Brewer.Interface;
using CoffeeMachine.CustomException;

namespace CoffeeMachine.Brewer
{
    public class BrewService : IBrewService
    {
        private const string BREW_MESSAGE = "Your piping hot coffee is ready";

        private readonly ICoffeeTimer _timer;
        private readonly ICoffeeBrewer _brewer;
        public BrewService(ICoffeeTimer timer, ICoffeeBrewer brewer)
        {
            _timer = timer;
            _brewer = brewer;
        }
        public Coffee Brew()
        {
            var preparedTime = _timer.GetPreparedTime();
            if (IsAprilFool(preparedTime))
            {
                throw new TeaPotException();
            }

            if (!_brewer.IsSuccessfullyBrewed())
            {
                throw new OutOfCoffeeException();
            }

            var res = new Coffee
            {
                message = BREW_MESSAGE,
                prepared = preparedTime
            };

            return res;
        }

        private bool IsAprilFool(string dateTime)
        {
            var date = DateTime.Parse(dateTime);
            return date.Day == 1 && date.Month == 4;
        }
    }
}
