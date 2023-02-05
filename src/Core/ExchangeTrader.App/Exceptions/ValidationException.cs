using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.App.Exceptions
{
    public class ValidationException : Exception
    {        
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(ValidationFailure[] errors)
        {
            var dic = new Dictionary<string, string[]>();

            foreach (var error in errors)
            {
                if (!dic.ContainsKey(error.PropertyName))
                {
                    dic.Add(error.PropertyName,
                        errors.Where(x => x.PropertyName == error.PropertyName)
                            .Select(x => x.ErrorMessage).ToArray());
                }
            }
            Errors = dic;
        }
    }
}
