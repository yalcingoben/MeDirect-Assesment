using ExchangeTrader.Api.Middlewares;
using ExchangeTrader.App.Abstractions.Auth;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ExchangeTrader.Api.Filters
{
    public class AuthFilter : IAsyncActionFilter
    {
        private readonly IAuthenticationService _service;

        public AuthFilter(IAuthenticationService service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var apiKey = context.HttpContext.Request.Headers["X-Api-Key"].FirstOrDefault();            
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                var error = new ErrorDetails()
                {
                    Message = "api key is required",
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                await ApiKeyExceptionAsync(context, error);
            }
            else if (!(await _service.IsApiKeyAvailable(apiKey)))
            {
                var error = new ErrorDetails()
                {
                    Message = "api key not found",
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                await ApiKeyExceptionAsync(context, error);
            }                            
            else
            {
                context.HttpContext.Items.TryAdd("apiKey", apiKey);                
                await next();
            }
        }
        
        private async Task ApiKeyExceptionAsync(ActionExecutingContext context, ErrorDetails error)
        {
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = error.StatusCode;            
            await context.HttpContext.Response.WriteAsync(error.ToString());
        }
    }
}
