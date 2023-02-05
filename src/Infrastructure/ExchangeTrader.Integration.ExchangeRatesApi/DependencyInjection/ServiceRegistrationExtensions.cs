using ExchangeTrader.App.Abstractions.Exchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTrader.Integration.ExchangeRatesApi.DependencyInjection
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddExchangeRateApi(this IServiceCollection services, IConfiguration configuration)
        {
            var exchangeRateConfiguration = configuration.GetSection("ExchangeRateApiConfiguration").Get<ExchangeRateApiConfiguration>();

            services.AddHttpClient("exchangeRateApi", c =>
            {
                c.BaseAddress = new Uri(exchangeRateConfiguration.Url);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("apikey", exchangeRateConfiguration.ApiKey);
                c.Timeout = new TimeSpan(0, 0, 30);
            });
            services.AddSingleton<IExchangeService, ExchangeRatesApiExchangeService>();
            return services;
        }
    }
}
