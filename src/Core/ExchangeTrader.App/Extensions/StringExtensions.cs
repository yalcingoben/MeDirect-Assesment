using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsInvariantCulture(this string value, string target)
            => value.Equals(target, StringComparison.InvariantCultureIgnoreCase);
    }
}
