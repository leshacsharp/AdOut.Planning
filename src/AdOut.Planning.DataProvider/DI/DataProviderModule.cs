﻿using AdOut.Planning.DataProvider.Context;
using AdOut.Planning.DataProvider.Repositories;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AdOut.Planning.DataProvider.DI
{
    public static class DataProviderModule
    {
        public static void AddDataProviderModule(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext, PlanningContext>();

            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
        }
    }
}