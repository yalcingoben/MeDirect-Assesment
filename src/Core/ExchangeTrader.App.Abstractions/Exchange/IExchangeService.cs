using ExchangeTrader.App.Abstractions.Exchange.Dtos;

namespace ExchangeTrader.App.Abstractions.Exchange
{
    public interface IExchangeService
    {
        Task<ExchangeRate> GetRates(string baseCurrency, CancellationToken cancellationToken);
    }
}
