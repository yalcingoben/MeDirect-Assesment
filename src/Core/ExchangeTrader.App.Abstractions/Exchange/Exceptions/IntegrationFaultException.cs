using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Abstractions.Exchange.Exceptions
{
    public class IntegrationFaultException : Exception
    {
        public int StatusCode { get; set; }

        public string Provider { get; set; }

        public IntegrationFaultException(string message, int statusCode, string provider) : base(message)
        {
            StatusCode = statusCode;
            Provider = provider;
        }
    }
}
