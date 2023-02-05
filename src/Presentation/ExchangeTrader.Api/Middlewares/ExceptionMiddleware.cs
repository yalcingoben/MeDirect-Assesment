using ExchangeTrader.App.Abstractions.Exchange.Exceptions;
using ExchangeTrader.App.Exceptions;
using System.Net;
using System.Text.Json;

namespace ExchangeTrader.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(ValidationException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(ex.Errors));
            }
            catch (CurrencyNotSupportedException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync(new ErrorDetails 
                { 
                    StatusCode = httpContext.Response.StatusCode, 
                    Message = ex.Message 
                }.ToString());
            }
            catch(IntegrationFaultException ex)
            {
                _logger.LogError("{provider}{statusCode}{errorMessage}", ex.Provider, ex.StatusCode, ex.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = ex.StatusCode;
                await httpContext.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = ex.Message
                }.ToString());                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await httpContext.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = ex.Message
                }.ToString());
            }
        }
    }
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
