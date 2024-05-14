using CoffeeMachine.Brewer.Interface;
using CoffeeMachine.Brewer.Model;
using CoffeeMachine.CustomException;
using CoffeeMachine.Infrastracuture;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CoffeeMachine.Brewer
{
    public class BrewService : IBrewService
    {
        private const string BREW_MESSAGE = "Your piping hot coffee is ready";
        private const string ICED_COFFEE_MESSAGE = "Your refreshing iced coffee is ready";
        private const int ICED_COFFEE_TEMPERATURE = 30;
        private const string LATITUDE_KEY = "GeologicalSetting:NZ:AKL:Latitude";
        private const string LONGITUDE_KEY = "GeologicalSetting:NZ:AKL:Longitude";
        private const string WEATHER_API_KEY = "WeatherApiKey";
        private const string BASE_URL = "WeatherBaseUrl";

        private readonly ICoffeeTimer _timer;
        private readonly ICoffeeBrewer _brewer;
        private readonly IHttpClientService _httpClient;
        private readonly IConfiguration _configuration;

        public BrewService(
            ICoffeeTimer timer, 
            ICoffeeBrewer brewer,
            IHttpClientService httpClient,
            IConfiguration configuration)
        {
            _timer = timer;
            _brewer = brewer;
            _httpClient = httpClient;
            _configuration = configuration;
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
                message = BREW_MESSAGE,
                prepared = preparedTime
            };

            if (await ShouldMakeIcedCoffee())
            {
                res.message = ICED_COFFEE_MESSAGE;
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
            return await GetCurrentTemperature() >= ICED_COFFEE_TEMPERATURE;
        }

        private async Task<int> GetCurrentTemperature()
        {
            Uri weatherUrl = ComposeCurrentTemperatureUrl();
            var weather = await _httpClient.GetAsync<Weather>(weatherUrl);
            
            return Convert.ToInt32(weather.main.temp);
        }

        private Uri ComposeCurrentTemperatureUrl()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("lat", _configuration[LATITUDE_KEY]);
            parameters.Add("lon", _configuration[LONGITUDE_KEY]);
            parameters.Add("appid", _configuration[WEATHER_API_KEY]);
            parameters.Add("units", "metric");

            var baseUrl = _configuration[BASE_URL];

            var uriBuilder = new UriBuilder(baseUrl);
            uriBuilder.Query = queryBuilder(parameters);

            return uriBuilder.Uri;
        }

        private string queryBuilder(Dictionary<string, string> parameters)
        {
            var builder = new StringBuilder();
            foreach ( var parameter in parameters ) { 
                builder.Append(parameter.Key)
                    .Append("=")
                    .Append(parameter.Value)
                    .Append("&");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
    }
}
