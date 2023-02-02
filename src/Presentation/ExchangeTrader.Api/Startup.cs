using ExchangeTrader.App;
using ExchangeTrader.App.Configurations;
using ExchangeTrader.Integration.DI;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTrader.Api
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var exchangeRateApiConfiguration = configRoot.GetSection("ExchangeRateApiConfiguration").Get<ExchangeRateApiConfiguration>();
            var fixerConfiguration = configRoot.GetSection("FixerConfiguration").Get<FixerConfiguration>();

            services.AddSingleton<ExchangeRateApiConfiguration>(p => configRoot.GetSection("ExchangeRateApiConfiguration").Get<ExchangeRateApiConfiguration>());
            services.AddSingleton<FixerConfiguration>(p => configRoot.GetSection("FixerConfiguration").Get<FixerConfiguration>());

            services.AddHttpClient("fixer", c =>
            {
                c.BaseAddress = new Uri(fixerConfiguration.Url);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("apikey", exchangeRateApiConfiguration.ApiKey);
                c.Timeout = new TimeSpan(0, 0, 30);
            });
            services.AddHttpClient("exchangeRateApi", c =>
            {
                c.BaseAddress = new Uri(exchangeRateApiConfiguration.Url);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("apikey", exchangeRateApiConfiguration.ApiKey);
                c.Timeout = new TimeSpan(0, 0, 30);
            });

            services.AddApplicationRegistration();
            services.AddRegisterIntegrations();

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
