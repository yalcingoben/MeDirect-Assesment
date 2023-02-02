using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.Integration.ExchangeRatesApi.Models
{
    internal class SymbolsResponse
    {
        public bool Success { get; set; }
        public Dictionary<string, string> Symbols { get; set; }
    }
}
