using ExchangeTrader.App.Features.Rate.Query;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Features.Rate.Validator
{
    public class GetRatesQueryValidator : AbstractValidator<GetRatesQuery>
    {
        public GetRatesQueryValidator()
        {
            RuleFor(x=> x.BaseCurrency).NotEmpty().Length(3);
        }
    }
}
