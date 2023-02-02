using ExchangeTrader.App.Dtos;

namespace ExchangeTrader.App.Abstractions.Services
{
    public interface IExchangeService
    {
        Task<ExchangeRateDto> GetRates(string baseCurrency);
        Task<IEnumerable<SymbolDto>> GetSymbols();
    }    
}
