using ExchangeTrader.App.Abstractions.Services;
using ExchangeTrader.App.Features.Symbol.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Features.Symbol.Query
{
    public class GetSymbolsQueryHandler : IRequestHandler<GetSymbolsQuery, GetSymbolsQueryResponse>
    {
        private readonly IExchangeService _exchangeService;

        public GetSymbolsQueryHandler(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }
        public async Task<GetSymbolsQueryResponse> Handle(GetSymbolsQuery request, CancellationToken cancellationToken)
        {
            var result = await _exchangeService.GetSymbols();
            return new GetSymbolsQueryResponse
            {
                Symbols = result,
            };
        }
    }
}
