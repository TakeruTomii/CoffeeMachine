namespace CoffeeMachine.Brewer
{
    public class CoffeeBrewer : ICoffeeBrewer
    {
        private static int counter = 0;
        private const int MAX_BREW = 5;
        public bool IsSuccessfullyBrewed()
        {
            counter++;
            if(counter >= MAX_BREW)
            {
                counter = 0;
                return false;
            }
            return true;
        }
    }
}
