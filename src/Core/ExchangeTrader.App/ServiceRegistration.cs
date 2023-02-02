using ExchangeTrader.App.Abstractions.Services;
using ExchangeTrader.App.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ExchangeTrader.App
{
    public static class ServiceRegistration
    {
        public static void AddApplicationRegistration(this IServiceCollection services)
        {
            var asmblies = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(asmblies);
            services.AddMediatR(asmblies);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
