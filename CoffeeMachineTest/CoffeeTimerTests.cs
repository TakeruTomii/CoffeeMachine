using CoffeeMachine.Brewer;
using System.Globalization;

namespace CoffeeMachineTest
{
    [TestClass]
    public class CoffeeTimerTests
    {
        private const string ISO_PATTERN = "yyyy-MM-ddTHH:mm:sszzz";

        [TestMethod]
        public void GetPreparedTime_ExcecuteOnce_ReturnISOFomatString()
        {
            var _timer = new CoffeeTimer();
            var res = _timer.GetPreparedTime();

            Assert.IsTrue(IsIso8601DateTime(res));
        }

        static bool IsIso8601DateTime(string input)
        {
            DateTime result;
            return DateTime.TryParseExact(input, ISO_PATTERN, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }
    }
}