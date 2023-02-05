using ExchangeTrader.App.Abstractions.Exchange.Enums;

namespace ExchangeTrader.App.Abstractions.Exchange.Configurations
{
    public class ExchangeCurrencyConfiguration
    {
        public string BaseSystemCurrency { get; set; } = "EUR";
        public TimeSpan CurrencyRateTimeout { get; set; }
        public ExchangeIntegrationProvider ExchangeIntegrationProvider { get; set; }
    }
}
