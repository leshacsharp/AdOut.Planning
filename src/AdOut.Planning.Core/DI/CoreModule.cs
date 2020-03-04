using AdOut.Planning.Core.Content.Factories;
using AdOut.Planning.Model.Interfaces.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace AdOut.Planning.Core.DI
{
    public static class CoreModule
    {
        public static void AddCoreModule (this IServiceCollection services)
        {
            services.AddScoped<IContentComponentsProvider, ContentComponentsProvider>();
        }
    }
}
