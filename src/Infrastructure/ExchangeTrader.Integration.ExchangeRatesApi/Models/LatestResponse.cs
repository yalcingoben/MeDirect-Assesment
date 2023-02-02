using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.Integration.ExchangeRatesApi.Models
{
    internal class LatestResponse
    {
        public bool Success { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, double> Rates { get; set; }

    }
}
