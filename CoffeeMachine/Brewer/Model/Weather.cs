using System.Text.Json.Serialization;

namespace CoffeeMachine.Brewer.Model
{
    public class Weather
    {
        [JsonPropertyName("main.temp")]
        public int Temperature { get; set; }
    }
}
