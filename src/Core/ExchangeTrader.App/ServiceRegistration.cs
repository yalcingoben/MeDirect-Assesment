using ExchangeTrader.App.Abstractions.Auth;
using ExchangeTrader.App.Abstractions.Auth.Configuratios;
using ExchangeTrader.App.Abstractions.Exchange;
using ExchangeTrader.App.Abstractions.Exchange.Configurations;
using ExchangeTrader.App.Auth;
using ExchangeTrader.App.Features.Trade.Services;
using ExchangeTrader.App.Pipelines;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ExchangeTrader.App
{
    public static class ServiceRegistration
    {
        public static void AddApplicationRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
            var asmblies = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(asmblies);
            services.AddMediatR(asmblies);
            services.AddValidatorsFromAssembly(asmblies);

            var apiKeyConfig = configuration.GetSection("ApiKeyConfiguration").Get<ApiKeyConfiguration>();
            ArgumentNullException.ThrowIfNull(apiKeyConfig);
            services.AddSingleton(apiKeyConfig);

            services.AddSingleton<IAuthenticationService, ConfigAuthenticationService>();
            services.AddScoped<IRateConverterService, RateConverterService>();            
        }
    }
}
