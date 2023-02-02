using ExchangeTrader.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Features.Symbol.Models
{
    public class GetSymbolsQueryResponse
    {
        public IEnumerable<SymbolDto> Symbols { get; set; }
    }
}
