using ExchangeTrader.App.Abstractions.Services;
using ExchangeTrader.App.Features.Rate.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Features.Rate.Query
{
    public class GetRatesQueryHandler : IRequestHandler<GetRatesQuery, GetRatesQueryResponse>
    {
        private readonly IExchangeService _exchangeService;

        public GetRatesQueryHandler(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        public async Task<GetRatesQueryResponse> Handle(GetRatesQuery request, CancellationToken cancellationToken)
        {
            var result = await _exchangeService.GetRates(request.BaseCurrency);
            if(result.Rates == null)
            {

            }
            return new GetRatesQueryResponse
            {
                BaseCurrency = request.BaseCurrency,
                Rates = result.Rates
            };
        }
    }
}
