namespace CoffeeMachine.Infrastracuture
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(Uri url);
    }
}
