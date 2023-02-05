using MediatR;

namespace ExchangeTrader.App.Features.Trade.Models
{
    public class ExchangeTradeCommand : IRequest<ExchangeTradeResponse>
    {
        public string Base { get; set; }
        public string Target { get; set; }
        public double Amount { get; set; }
    }
}
