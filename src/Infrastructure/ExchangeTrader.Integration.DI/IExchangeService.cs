using ExchangeTrader.Integration.Abstractions.Dtos;

namespace ExchangeTrader.Integration.Abstractions
{
    public interface IExchangeService
    {
        Task<ExchangeRate> GetRates(string baseCurrency);
        Task<IEnumerable<Symbol>> GetSymbols();
    }    
}
