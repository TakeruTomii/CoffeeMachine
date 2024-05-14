using CoffeeMachine.Brewer.Interface;
using CoffeeMachine.Brewer.Model;
using CoffeeMachine.CustomException;
using CoffeeMachine.Infrastracuture;
using System.Text;

namespace CoffeeMachine.Brewer
{
    public class BrewService : IBrewService
    {
        private readonly ICoffeeTimer _timer;
        private readonly ICoffeeBrewer _brewer;
        private readonly IHttpClientService _httpClient;
        private readonly string _weatherApiKey;

        public BrewService(
            ICoffeeTimer timer,
            ICoffeeBrewer brewer,
            IHttpClientService httpClient,
            IConfiguration configuration)
        {
            _timer = timer;
            _brewer = brewer;
            _httpClient = httpClient;
            _weatherApiKey = configuration[BrewerConstants.KEY_WEATHER_API_KEY];
        }
        public async Task<Coffee> Brew()
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
                message = BrewerConstants.BREW_MESSAGE,
                prepared = preparedTime
            };

            if (await ShouldMakeIcedCoffee())
            {
                res.message = BrewerConstants.ICED_COFFEE_MESSAGE;
            }

            return res;
        }

        private bool IsAprilFool(string dateTime)
        {
            var date = DateTime.Parse(dateTime);
            return date.Day == 1 && date.Month == 4;
        }

        private async Task<bool> ShouldMakeIcedCoffee()
        {
            return await GetCurrentTemperature() >= BrewerConstants.ICED_COFFEE_TEMPERATURE;
        }

        private async Task<int> GetCurrentTemperature()
        {
            Uri weatherUrl = ComposeCurrentTemperatureUrl();
            var weather = await _httpClient.GetAsync<Weather>(weatherUrl);
            return Convert.ToInt32(Math.Floor(weather.main.temp));
        }

        private Uri ComposeCurrentTemperatureUrl()
        {
            var parameters = new Dictionary<string, string>
            {
                { BrewerConstants.PARAM_LATITUDE, BrewerConstants.NZ_LATITUDE.ToString() },
                { BrewerConstants.PARAM_LONGITUDE, BrewerConstants.NZ_LONGITUDE.ToString() },
                { BrewerConstants.PARAM_APIKEY, _weatherApiKey },
                { BrewerConstants.PARAM_UNITS, BrewerConstants.UNITS_VALUE }
            };

            var uriBuilder = new UriBuilder(BrewerConstants.WEATHER_BASE_URL);
            uriBuilder.Query = queryBuilder(parameters);

            return uriBuilder.Uri;
        }

        private string queryBuilder(Dictionary<string, string> parameters)
        {
            var builder = new StringBuilder();
            foreach (var parameter in parameters)
            {
                builder.Append(parameter.Key)
                    .Append(BrewerConstants.CONNECTOR_KEY_VALUE)
                    .Append(parameter.Value)
                    .Append(BrewerConstants.CONNECTOR_PARAMS);
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
    }
}
