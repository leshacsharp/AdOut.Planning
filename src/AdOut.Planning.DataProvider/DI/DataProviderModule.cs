using AdOut.Planning.DataProvider.Context;
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
            services.AddScoped<ICommitProvider, CommitProvider>();

            services.AddScoped<IAdPointRepository, AdPointRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IPlanAdRepository, PlanAdRepository>();
            services.AddScoped<IPlanAdPointRepository, PlanAdPointRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IAdRepository, AdRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<ITariffRepository, TariffRepository>();
        }
    }
}
