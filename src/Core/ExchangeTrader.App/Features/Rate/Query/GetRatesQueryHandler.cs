using ExchangeTrader.App.Abstractions.Caching;
using ExchangeTrader.App.Abstractions.Exchange;
using ExchangeTrader.App.Abstractions.Exchange.Configurations;
using ExchangeTrader.App.Abstractions.Exchange.Dtos;
using ExchangeTrader.App.Dtos;
using ExchangeTrader.App.Features.Rate.Models;
using MediatR;

namespace ExchangeTrader.App.Features.Rate.Query
{
    public class GetRatesQueryHandler : IRequestHandler<GetRatesQuery, GetRatesQueryResponse>
    {
        private readonly IExchangeService _exchangeService;
        private readonly ICacheService _cacheService;
        private readonly ExchangeCurrencyConfiguration _exchangeCurrencyConfiguration;        

        public GetRatesQueryHandler(
            IExchangeService exchangeService, 
            ICacheService cacheService, 
            ExchangeCurrencyConfiguration exchangeCurrencyConfiguration)
        {
            _exchangeService = exchangeService;
            _cacheService = cacheService;
            _exchangeCurrencyConfiguration = exchangeCurrencyConfiguration;
        }

        public async Task<GetRatesQueryResponse> Handle(GetRatesQuery request, CancellationToken cancellationToken)
        {
            var baseCurrency = request.BaseCurrency.ToUpper();
            var result = await _cacheService.GetOrAddAsync<ExchangeRate>(
                $"CurrencyExchangeRatesBased{baseCurrency}",
                () => { return _exchangeService.GetRates(request.BaseCurrency, cancellationToken); },
                _exchangeCurrencyConfiguration.CurrencyRateTimeout, 
                cancellationToken);

            ArgumentNullException.ThrowIfNull(result, nameof(result));
            ArgumentNullException.ThrowIfNull(result.Rates, nameof(result.Rates));            
            return new GetRatesQueryResponse
            {
                BaseCurrency = baseCurrency,
                Rates = result.Rates.Select(c=> new RateDto { Code = c.Code, Value = c.Value })
            };
        }
    }
}
