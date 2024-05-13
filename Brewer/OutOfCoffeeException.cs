namespace CoffeeMachine.Brewer
{
    public class OutOfCoffeeException : Exception
    {
        public OutOfCoffeeException(string message) : base(message){}
    }
}
