using ExchangeTrader.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Features.Rate.Models
{
    public class GetRatesQueryResponse
    {
        public string BaseCurrency { get; set; }
        public IEnumerable<RateDto> Rates { get; set; }
    }
}
