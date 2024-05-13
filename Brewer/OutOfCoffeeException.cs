namespace CoffeeMachine.Brewer
{
    public class OutOfCoffeeException : Exception
    {
        private const string defaultMessage = "Sorry, we are out of coffee";
        public OutOfCoffeeException() : base(defaultMessage) { }
        public OutOfCoffeeException(string message) : base(message) { }
    }
}
