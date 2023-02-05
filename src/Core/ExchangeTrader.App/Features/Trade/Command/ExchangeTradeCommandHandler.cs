using ExchangeTrader.App.Abstractions.Exchange;
using ExchangeTrader.App.Abstractions.Exchange.Configurations;
using ExchangeTrader.App.Abstractions.Exchange.Dtos;
using ExchangeTrader.App.Exceptions;
using ExchangeTrader.App.Extensions;
using ExchangeTrader.App.Features.Rate.Models;
using ExchangeTrader.App.Features.Rate.Query;
using ExchangeTrader.App.Features.Trade.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExchangeTrader.App.Features.Trade.Command
{
    public class ExchangeTradeCommandHandler : IRequestHandler<ExchangeTradeCommand, ExchangeTradeResponse>
    {
        private readonly IMediator mediator;
        private readonly ExchangeCurrencyConfiguration exchangeCurrencyConfiguration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRateConverterService rateConverterService;
        private readonly ILogger<ExchangeTradeCommandHandler> logger;

        public ExchangeTradeCommandHandler(
            IMediator mediator,
            ExchangeCurrencyConfiguration exchangeCurrencyConfiguration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ExchangeTradeCommandHandler> logger,
            IRateConverterService rateConverterService)
        {
            this.mediator = mediator;
            this.exchangeCurrencyConfiguration = exchangeCurrencyConfiguration;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.rateConverterService = rateConverterService;
        }

        public async Task<ExchangeTradeResponse> Handle(ExchangeTradeCommand request, CancellationToken cancellationToken)
        {
            var ratesQueryResponse = await mediator.Send(new GetRatesQuery { BaseCurrency = exchangeCurrencyConfiguration.BaseSystemCurrency });
            ArgumentNullException.ThrowIfNull(nameof(ratesQueryResponse));
            ThrowExceptionIfNotSupported(request.Base, ratesQueryResponse);
            ThrowExceptionIfNotSupported(request.Target, ratesQueryResponse);

            var rateConverterData = new RateConverterData
            {
                BaseCurrency = ratesQueryResponse.BaseCurrency,
                Rates = ratesQueryResponse.Rates.Select(x => new Abstractions.Exchange.Dtos.Rate
                {
                    Code = x.Code,
                    Value = x.Value
                })
            };

            var conversionRate = rateConverterService.Convert(request.Base, request.Target, rateConverterData);
            var convertedAmount = conversionRate * request.Amount;
            if (conversionRate > 0 && httpContextAccessor.HttpContext.Items.TryGetValue("apiKey", out var value))
            {
                _ = Task.Run(() =>
                {
                    logger.LogInformation("{apiKey}{base}{target}{amount}{conversionRate}{convertedAmount}",
                    value, request.Base, request.Target, request.Amount, conversionRate, convertedAmount);
                }, cancellationToken);
                
            }
            return new ExchangeTradeResponse { Success = true, ConvertedAmount = convertedAmount };
        }

        private void ThrowExceptionIfNotSupported(string currency, GetRatesQueryResponse queryResponse)
        {
            if(!queryResponse.BaseCurrency.EqualsInvariantCulture(currency) && !queryResponse.Rates.Any(x => x.Code.EqualsInvariantCulture(currency)))
                throw new CurrencyNotSupportedException($"{currency} is not supported currency");
        }
    }
}
