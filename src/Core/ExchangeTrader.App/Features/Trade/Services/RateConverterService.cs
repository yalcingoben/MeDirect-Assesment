using ExchangeTrader.App.Abstractions.Exchange;
using ExchangeTrader.App.Abstractions.Exchange.Dtos;
using ExchangeTrader.App.Extensions;

namespace ExchangeTrader.App.Features.Trade.Services
{
    public class RateConverterService : IRateConverterService
    {
        public double Convert(string baseCurrency, string targetCurrency, RateConverterData rateConverterData)
        {
            if (baseCurrency.EqualsInvariantCulture(rateConverterData.SystemCurrency))
            {
                return rateConverterData.Rates.First(x => x.Code.EqualsInvariantCulture(targetCurrency)).Value;
            }
            else if (targetCurrency.EqualsInvariantCulture(rateConverterData.SystemCurrency))
            {
                return 1 / rateConverterData.Rates.First(x => x.Code.EqualsInvariantCulture(baseCurrency)).Value;
            }

            var currencyTargetPair = rateConverterData.Rates.First(x => x.Code.EqualsInvariantCulture(targetCurrency));
            var currencyBasePair = rateConverterData.Rates.First(x => x.Code.EqualsInvariantCulture(baseCurrency));
            return currencyTargetPair.Value / currencyBasePair.Value;
        }
    }
}
