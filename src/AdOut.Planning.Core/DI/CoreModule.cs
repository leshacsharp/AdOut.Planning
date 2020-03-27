using AdOut.Planning.Core.Content.Providers;
using AdOut.Planning.Core.Content.Storages;
using AdOut.Planning.Core.Managers;
using AdOut.Planning.Core.Schedule.Factories;
using AdOut.Planning.Core.Schedule.Helpers;
using AdOut.Planning.Core.Schedule.Providers;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Schedule;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace AdOut.Planning.Core.DI
{
    public static class CoreModule
    {
        public static void AddCoreModule (this IServiceCollection services)
        {
            services.AddScoped<IAdManager, AdManager>();
            services.AddScoped<IScheduleManager, ScheduleManager>();
            services.AddScoped<IPlanManager, PlanManager>();

            var scheduleValidators = Assembly.GetCallingAssembly()
                            .GetTypes()
                            .Where(t => t.GetInterfaces().Any(i => i == typeof(IScheduleValidator)) && !t.IsAbstract)
                            .ToList();

            foreach (var scheduleValidator in scheduleValidators)
            {
                services.AddScoped(typeof(IScheduleValidator), scheduleValidator);
            }


            services.AddScoped<IScheduleValidatorFactory, ScheduleValidatorFactory>();
            services.AddScoped<IScheduleTimeHelperProvider, ScheduleTimeHelperProvider>();
            services.AddScoped<ITimeLineHelper, TimeLineHelper>();

            services.AddScoped<IDirectoryDistributor, EmptyDirectoryDistributor>();
            services.AddScoped<IContentStorage, LocalStorage>();
            services.AddScoped<IContentHelperProvider, ContentHelperProvider>();
            services.AddScoped<IContentValidatorProvider, ContentValidatorProvider>();
        }
    }
}
