using ExchangeTrader.App.Abstractions.Exchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTrader.Integration.Fixer.DependencyInjection
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddFixer(this IServiceCollection services, IConfiguration configuration)
        {
            var fixerConfiguration = configuration.GetSection("FixerConfiguration").Get<FixerConfiguration>();

            services.AddHttpClient("fixer", c =>
            {
                c.BaseAddress = new Uri(fixerConfiguration.Url);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("apikey", fixerConfiguration.ApiKey);
                c.Timeout = new TimeSpan(0, 0, 30);
            });
            services.AddSingleton<IExchangeService, FixerExchangeService>();
            return services;
        }
    }
}
