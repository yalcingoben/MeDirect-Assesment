using ExchangeTrader.App.Features.Symbol.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Features.Symbol.Query
{
    public class GetSymbolsQuery :IRequest<GetSymbolsQueryResponse>
    {
    }
}
