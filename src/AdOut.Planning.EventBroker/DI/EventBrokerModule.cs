using AdOut.Planning.Model.Interfaces.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace AdOut.Planning.EventBroker.DI
{
    public static class EventBrokerModule
    {
        public static void AddEventBrokerModule(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionManager, RabbitConnectionManager>();
            services.AddScoped<IEventBroker, RabbitEventBroker>();
            services.AddScoped<IEventBrokerHelper, EventBrokerHelper>();
            services.AddScoped<IEventBinder, EventBinder>();
        }
    }
}
