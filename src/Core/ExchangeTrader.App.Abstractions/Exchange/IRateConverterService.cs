using ExchangeTrader.App.Abstractions.Exchange.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Abstractions.Exchange
{
    public interface IRateConverterService
    {
        double Convert(string baseCurrency, string targetCurrency, RateConverterData rateConverterData);
    }
}
