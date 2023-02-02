using ExchangeTrader.App.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Dtos
{
    public class ExchangeRateDto
    {
        public List<RateDto> Rates { get; set; }
        public DateTime Date { get; set; }
    }
}
