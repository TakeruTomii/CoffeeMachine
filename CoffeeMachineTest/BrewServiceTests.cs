using CoffeeMachine.Brewer;
using CoffeeMachine.Brewer.Interface;
using CoffeeMachine.Brewer.Model;
using CoffeeMachine.CustomException;
using Moq;

namespace CoffeeMachineTest
{
    [TestClass]
    public class BrewServiceTests
    {
        private const string BREW_MESSAGE = "Your piping hot coffee is ready";
        private const string VALID_DATE = "2021-02-03T11:56:24+0900";
        private const string APRIL_FOOL = "2021-04-01T11:56:24+0900";
        private IBrewService _service;
        private Mock<ICoffeeTimer> _timer;
        private Mock<ICoffeeBrewer> _brewer;

        [TestInitialize]
        public void Initialize()
        {
            _timer = new Mock<ICoffeeTimer>();
            _brewer = new Mock<ICoffeeBrewer>();
        }

        [TestMethod]
        public void Brew_ExcecuteOnce_ReturnCoffeeInfo()
        {
            var expectedRes = new Coffee { message = BREW_MESSAGE, prepared = VALID_DATE };
            _timer.Setup(x => x.GetPreparedTime()).Returns(VALID_DATE);
            _brewer.Setup(x => x.IsSuccessfullyBrewed()).Returns(true);

            _service = new BrewService(_timer.Object, _brewer.Object);
            var res = _service.Brew();

            Assert.AreEqual(expectedRes.message, res.message);
            Assert.AreEqual(expectedRes.prepared, res.prepared);
        }

        [TestMethod]
        public void Brew_ExcecuteFiveTimes_ThrowsOutOfCoffeeExceptions()
        {
            _timer.Setup(x => x.GetPreparedTime()).Returns(VALID_DATE);
            _brewer.SetupSequence(x => x.IsSuccessfullyBrewed())
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(false);

            _service = new BrewService(_timer.Object, _brewer.Object);

            for (var i = 0; i < 4; i++)
            {
                _service.Brew();
            }

            Assert.ThrowsException<OutOfCoffeeException>(() =>
            {
                _service.Brew();
            });
        }

        [TestMethod]
        public void Brew_ExcecuteInAprilFool_ThrowsTeaPotException()
        {
            _timer.Setup(x => x.GetPreparedTime()).Returns(APRIL_FOOL);
            _brewer.Setup(x => x.IsSuccessfullyBrewed()).Returns(true);

            _service = new BrewService(_timer.Object, _brewer.Object);
            Assert.ThrowsException<TeaPotException>(() =>
            {
                _service.Brew();
            });
        }
    }
}