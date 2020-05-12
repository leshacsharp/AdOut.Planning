using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AdOut.Planning.WebApi.DI
{
    public static class WebApiModule
    {
        public static void AddWebApiModule(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, ResourceAuthorizationHandler<AdDto>>();
            services.AddScoped<IAuthorizationHandler, ResourceAuthorizationHandler<Ad>>();
            services.AddScoped<IAuthorizationHandler, ResourceAuthorizationHandler<PlanDto>>();
            services.AddScoped<IAuthorizationHandler, ResourceAuthorizationHandler<Plan>>();
        }
    }
}
