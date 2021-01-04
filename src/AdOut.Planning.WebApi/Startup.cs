using AdOut.Extensions.Communication;
using AdOut.Extensions.Communication.Interfaces;
using AdOut.Extensions.Filters;
using AdOut.Planning.DataProvider.Context;
using AdOut.Planning.Model.Settings;
using AdOut.Planning.WebApi.Configuration;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

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
            services.AddHttpContextAccessor();

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
                     options.UseSqlServer(Configuration.GetConnectionString("DevConnection")).EnableSensitiveDataLogging());

            services.AddDataProviderServices();
            services.AddCoreServices();
            services.AddMessageBrokerServices();

            services.Configure<AWSS3Config>(Configuration.GetSection(nameof(AWSS3Config)));
            services.Configure<RabbitConfig>(Configuration.GetSection(nameof(RabbitConfig)));

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
                endpoints.MapControllers();
            });

            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PlanningContext>();
            var messageBroker = scope.ServiceProvider.GetRequiredService<IMessageBroker>();
            var consumerBinder = scope.ServiceProvider.GetRequiredService<IConsumerBinder>();

            context.Database.Migrate();
            messageBroker.Configure();
            consumerBinder.Bind();
        }
    }
}
