﻿using CoffeeMachine.Brewer.Interface;

namespace CoffeeMachine.Brewer
{
    public class CoffeeTimer : ICoffeeTimer
    {
        private const string ISO_8601_DATE_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";
        public string GetPreparedTime()
        {
            return DateTime.UtcNow.ToString(ISO_8601_DATE_FORMAT);
        }
    }
}
