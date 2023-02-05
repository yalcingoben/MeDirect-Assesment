using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Features.Trade.Models
{
    public class ExchangeTradeResponse
    {
        public bool Success { get; set; }

        public double ConvertedAmount { get; set; }
    }
}
