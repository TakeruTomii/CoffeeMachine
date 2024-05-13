using CoffeeMachine.Brewer;
using CoffeeMachine.Brewer.Interface;
using CoffeeMachine.Brewer.Model;
using CoffeeMachine.CustomException;
using Moq;

namespace CoffeeMachineTest
{
    [TestClass]
    public class CoffeeBrewerTests
    {
        [TestMethod]
        public void GetPreparedTime_ExcecuteOnce_ReturnTrue()
        {
            var _brewer = new CoffeeBrewer();
            var res = _brewer.IsSuccessfullyBrewed();

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void GetPreparedTime_ExcecuteFiveTimes_ReturnFalse()
        {
            var _brewer = new CoffeeBrewer();
            for(var i = 0; i < 4; i++) {
                _brewer.IsSuccessfullyBrewed();
            }
            var res = _brewer.IsSuccessfullyBrewed();

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void GetPreparedTime_ExcecuteSixTimes_ReturnTrue()
        {
            var _brewer = new CoffeeBrewer();
            for (var i = 0; i < 5; i++)
            {
                _brewer.IsSuccessfullyBrewed();
            }
            var res = _brewer.IsSuccessfullyBrewed();

            Assert.IsTrue(res);
        }
    }
}