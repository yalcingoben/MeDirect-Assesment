using ExchangeTrader.App.Abstractions.Services;
using ExchangeTrader.Integration.ExchangeRatesApi;
using ExchangeTrader.Integration.Fixer;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTrader.Integration.DI
{
    public static class ServiceRegistration
    {
        public static void AddRegisterIntegrations(this IServiceCollection services)
        {
            services.AddScoped<IExchangeService, FixerExchangeService>();
        }
    }
}