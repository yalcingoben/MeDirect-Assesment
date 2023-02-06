using ExchangeTrader.Api.Filters;
using ExchangeTrader.App.Features.Trade.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeTrader.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IMediator _mediator;        

        public TradeController(IMediator mediator)
        {
            _mediator = mediator;            
        }
        [HttpPost]
        [ServiceFilter(typeof(AuthFilter))]
        public async Task<ExchangeTradeResponse> Trade([FromBody] ExchangeTradeCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
