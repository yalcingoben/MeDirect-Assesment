using AspNetCoreRateLimit;
using ExchangeTrader.Api.Extensions;
using ExchangeTrader.Api.Filters;
using ExchangeTrader.Api.Middlewares;
using ExchangeTrader.App;
using ExchangeTrader.App.Abstractions.Exchange.Configurations;
using ExchangeTrader.App.Abstractions.Exchange.Enums;
using ExchangeTrader.Caching.Redis.DependencyInjection;
using ExchangeTrader.Integration.ExchangeRatesApi.DependencyInjection;
using ExchangeTrader.Integration.Fixer.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

var exchangeCurrencyConfigModel = configuration.GetSection("ExchangeCurrencyConfiguration").Get<ExchangeCurrencyConfiguration>();
ArgumentNullException.ThrowIfNull(exchangeCurrencyConfigModel);
builder.Services.AddSingleton(exchangeCurrencyConfigModel);

if (exchangeCurrencyConfigModel.ExchangeIntegrationProvider == ExchangeIntegrationProvider.Fixer)
{
    builder.Services.AddFixer(configuration);
}
else
{
    builder.Services.AddExchangeRateApi(configuration);
}

builder.Services.AddRedis(configuration);
builder.Services.AddRateLimiting(configuration);
builder.Services.AddApplicationRegistration(configuration);

builder.Services.AddControllers();
builder.Services.AddScoped<AuthFilter>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseClientRateLimiting();

if (!app.Environment.IsDevelopment())
{    
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


