using AdOut.Planning.Core.DI;
using AdOut.Planning.Core.Settings;
using AdOut.Planning.DataProvider.Context;
using AdOut.Planning.DataProvider.DI;
using AdOut.Planning.WebApi.Filters;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace AdOut.Planning.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilterAttribute>();
                options.Filters.Add<ModelStateFilterAttribute>();
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = Configuration.GetValue<string>("Authorization:Authority");
                        options.ApiName = Configuration.GetValue<string>("Authorization:ApiName");
                        options.ApiSecret = Configuration.GetValue<string>("Authorization:ApiSecret");

                        options.EnableCaching = true;
                        options.CacheDuration = TimeSpan.FromSeconds(Configuration.GetValue<int>("Authorization:TokenCacheDurationSec"));

                        options.NameClaimType = "name";
                        options.RoleClaimType = "role";
                    });

            services.AddDbContext<PlanningContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));

            services.AddDataProviderModule();
            services.AddCoreModule();

            services.Configure<AWSS3Config>(Configuration.GetSection(nameof(AWSS3Config)));

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "AdOut.Planning API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "AdOut.Planning API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                         .RequireAuthorization();
            });
        }
    }
}
