using ExchangeTrader.App.Features.Trade.Models;
using FluentValidation;

namespace ExchangeTrader.App.Features.Trade.Validator
{
    public class ExchangeTradeCommandValidator : AbstractValidator<ExchangeTradeCommand>
    {
        public ExchangeTradeCommandValidator() 
        {
            RuleFor(x => x.Base).NotEmpty().Length(3);
            RuleFor(x => x.Target).NotEmpty().Length(3);
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Target).NotEqual(y=> y.Base);
        }
    }
}
