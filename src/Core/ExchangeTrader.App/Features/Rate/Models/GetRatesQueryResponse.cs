using ExchangeTrader.App.Dtos;

namespace ExchangeTrader.App.Features.Rate.Models
{
    public class GetRatesQueryResponse
    {
        public string BaseCurrency { get; set; }
        public IEnumerable<RateDto> Rates { get; set; }
    }
}
