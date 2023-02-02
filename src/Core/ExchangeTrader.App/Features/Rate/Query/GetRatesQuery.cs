using ExchangeTrader.App.Features.Rate.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Features.Rate.Query
{
    public class GetRatesQuery:IRequest<GetRatesQueryResponse>
    {
        public string BaseCurrency { get; set; }
    }
}
