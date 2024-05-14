using CoffeeMachine.Brewer.Model;

namespace CoffeeMachine.Brewer.Interface
{
    public interface IBrewService
    {
        Task<Coffee> Brew();
    }
}
