using System.Text.Json;
using System.Xml.Linq;

namespace CoffeeMachine.Infrastracuture
{
    public class HttpClientService
    {
        readonly HttpClient _httpClient;
        async Task<T> GetAsync<T>(Uri url)
        {
            var res = await _httpClient.GetAsync(url);

            res.EnsureSuccessStatusCode();

            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
