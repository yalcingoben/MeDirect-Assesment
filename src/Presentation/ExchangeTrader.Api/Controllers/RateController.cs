using ExchangeTrader.App.Features.Rate.Models;
using ExchangeTrader.App.Features.Rate.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeTrader.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "all")]
        public async Task<GetRatesQueryResponse> Get([FromQuery] string baseCurrency)
        {
            return await _mediator.Send(new GetRatesQuery { BaseCurrency = baseCurrency });
        }
    }
}
