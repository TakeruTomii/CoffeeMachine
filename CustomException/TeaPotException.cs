namespace CoffeeMachine.CustomException
{
    public class TeaPotException : Exception
    {
        private const string defaultMessage = "April fool: I'm a teapot";
        public TeaPotException() : base(defaultMessage) { }
        public TeaPotException(string message) : base(message) { }
    }
}
