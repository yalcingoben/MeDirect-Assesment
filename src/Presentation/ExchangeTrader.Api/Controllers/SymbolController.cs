using ExchangeTrader.App.Features.Symbol.Models;
using ExchangeTrader.App.Features.Symbol.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeTrader.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SymbolController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SymbolController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<GetSymbolsQueryResponse> Get()
        {
            return await _mediator.Send(new GetSymbolsQuery());
        }
    }
}
