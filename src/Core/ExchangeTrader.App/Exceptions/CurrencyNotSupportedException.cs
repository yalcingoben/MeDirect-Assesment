using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Exceptions
{
    public class CurrencyNotSupportedException : Exception
    {
        public CurrencyNotSupportedException(string currency): base($"{currency} is not supported currency")
        {

        }
    }
}
