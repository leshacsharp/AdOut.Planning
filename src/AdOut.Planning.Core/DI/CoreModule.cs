using AdOut.Planning.Core.Content.Helpers;
using AdOut.Planning.Core.Content.Storages;
using AdOut.Planning.Core.Content.Validators;
using AdOut.Planning.Core.EventHandlers;
using AdOut.Planning.Core.Managers;
using AdOut.Planning.Core.Schedule.Helpers;
using AdOut.Planning.Core.Schedule.Validators;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Schedule;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Linq;
using System.Reflection;

namespace AdOut.Planning.Core.DI
{
    public static class CoreModule
    {
        public static void AddCoreModule (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAdManager, AdManager>();
            services.AddScoped<IScheduleManager, ScheduleManager>();
            services.AddScoped<IPlanManager, PlanManager>();
            services.AddScoped<IPlanAdManager, PlanAdManager>();
            services.AddScoped<IUserManager, UserManager>();

            var scheduleValidators = Assembly.GetCallingAssembly()
                           .GetTypes()
                           .Where(t => t.GetInterfaces().Any(i => i == typeof(IScheduleValidator)) && !t.IsAbstract);

            foreach (var scheduleValidator in scheduleValidators)
            {
                services.AddScoped(typeof(IScheduleValidator), scheduleValidator);
            }

            services.AddScoped<IScheduleValidatorFactory, ScheduleValidatorFactory>();
            services.AddScoped<IScheduleTimeHelperProvider, ScheduleTimeHelperProvider>();

            //todo: uncomment in Production
            //var awsConfig = new AWSS3Config();
            //configuration.Bind(nameof(AWSS3Config), awsConfig);
            //var awsCredentials = new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey);
            //var regionEndpoint = RegionEndpoint.GetBySystemName(awsConfig.RegionEndpointName);
            //var awsClient = new AmazonS3Client(awsCredentials, regionEndpoint);

            //services.AddScoped<IContentStorage>(p => new AWSS3Storage(awsClient, awsConfig.BucketName));

            services.AddScoped<IContentStorage, LocalStorage>();
            services.AddScoped<IContentHelperProvider, ContentHelperProvider>();
            services.AddScoped<IContentValidatorProvider, ContentValidatorProvider>();

            services.AddSingleton<IBasicConsumer, AdPointCreatedConsumer>();
            services.AddSingleton<IBasicConsumer, AdPointDeletedConsumer>();
        }
    }
}
