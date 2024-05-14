using CoffeeMachine.Brewer;
using CoffeeMachine.Brewer.Interface;
using CoffeeMachine.Brewer.Model;
using CoffeeMachine.CustomException;
using CoffeeMachine.Infrastracuture;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CoffeeMachineTest
{
    [TestClass]
    public class BrewServiceTests
    {
        private const string BREW_MESSAGE = "Your piping hot coffee is ready";
        private const string VALID_DATE = "2021-02-03T11:56:24+0900";
        private const string APRIL_FOOL = "2021-04-01T11:56:24+0900";
        private const double HOT_COFFEE_TEMPERATURE = 29.9;
        private const double ICED_COFFEE_TEMPERATURE = 30.0;
        private IBrewService _service;
        private Mock<ICoffeeTimer> _timer;
        private Mock<ICoffeeBrewer> _brewer;
        private Mock<IHttpClientService> _httpClient;
        private Mock<IConfiguration> _configuration;

        [TestInitialize]
        public void Initialize()
        {
            _timer = new Mock<ICoffeeTimer>();
            _brewer = new Mock<ICoffeeBrewer>();
            _httpClient = new Mock<IHttpClientService>();
            _configuration = new Mock<IConfiguration>();
            _configuration
                .Setup(x => x[BrewerConstants.KEY_WEATHER_API_KEY])
                .Returns("test-api-key");
        }

        [TestMethod]
        public async Task Brew_ExcecuteOnce_ReturnCoffeeInfo()
        {
            var expectedRes = new Coffee { message = BREW_MESSAGE, prepared = VALID_DATE };
            var main = new Main() { temp = HOT_COFFEE_TEMPERATURE };
            var weather = new Weather() { main = main };
            _timer.Setup(x => x.GetPreparedTime()).Returns(VALID_DATE);
            _brewer.Setup(x => x.IsSuccessfullyBrewed()).Returns(true);
            _httpClient
                .Setup(x => x.GetAsync<Weather>(It.IsAny<Uri>()))
                .Returns(Task.FromResult(weather));

            _service = new BrewService(_timer.Object, _brewer.Object, _httpClient.Object, _configuration.Object);
            var res = await _service.Brew();

            Assert.AreEqual(expectedRes.message, res.message);
            Assert.AreEqual(expectedRes.prepared, res.prepared);
        }

        [TestMethod]
        public async Task Brew_ExcecuteFiveTimes_ThrowsOutOfCoffeeExceptions()
        {
            var main = new Main() { temp = HOT_COFFEE_TEMPERATURE };
            var weather = new Weather() { main = main };
            _timer.Setup(x => x.GetPreparedTime()).Returns(VALID_DATE);
            _brewer.SetupSequence(x => x.IsSuccessfullyBrewed())
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(false);
            _httpClient
                .Setup(x => x.GetAsync<Weather>(It.IsAny<Uri>()))
                .Returns(Task.FromResult(weather));

            _service = new BrewService(_timer.Object, _brewer.Object, _httpClient.Object, _configuration.Object);

            for (var i = 0; i < 4; i++)
            {
                await _service.Brew();
            }

            await Assert.ThrowsExceptionAsync<OutOfCoffeeException>(async () =>
            {
                await _service.Brew();
            });
        }

        [TestMethod]
        public async Task Brew_ExcecuteInAprilFool_ThrowsTeaPotException()
        {
            _timer.Setup(x => x.GetPreparedTime()).Returns(APRIL_FOOL);
            _brewer.Setup(x => x.IsSuccessfullyBrewed()).Returns(true);

            _service = new BrewService(_timer.Object, _brewer.Object, _httpClient.Object, _configuration.Object);
            await Assert.ThrowsExceptionAsync<TeaPotException>(async () =>
            {
                await _service.Brew();
            });
        }
    }
}