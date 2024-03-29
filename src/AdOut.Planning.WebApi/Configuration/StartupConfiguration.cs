﻿using AdOut.Extensions.Communication;
using AdOut.Extensions.Communication.Interfaces;
using AdOut.Extensions.Context;
using AdOut.Planning.Core.Consumers;
using AdOut.Planning.Core.Managers;
using AdOut.Planning.Core.Mapping;
using AdOut.Planning.Core.Replication;
using AdOut.Planning.Core.Services.Content;
using AdOut.Planning.Core.Services.Schedule;
using AdOut.Planning.Core.Validators.Content;
using AdOut.Planning.Core.Validators.Schedule;
using AdOut.Planning.DataProvider.Context;
using AdOut.Planning.DataProvider.Repositories;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Linq;
using System.Reflection;

namespace AdOut.Planning.WebApi.Configuration
{
    public static class StartupConfiguration
    {
        public static void AddDataProviderServices(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext>(p => p.GetRequiredService<PlanningContext>());
            services.AddScoped<ICommitProvider, CommitProvider<PlanningContext>>();

            services.AddScoped<IAdPointRepository, AdPointRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IPlanAdRepository, PlanAdRepository>();
            services.AddScoped<IPlanAdPointRepository, PlanAdPointRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IAdRepository, AdRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<ITariffRepository, TariffRepository>();
            services.AddScoped<IPlanTimeRepository, PlanTimeRepository>();

            services.AddScoped<IAdManager, AdManager>();
            services.AddScoped<IScheduleManager, ScheduleManager>();
            services.AddScoped<IPlanManager, PlanManager>();
            services.AddScoped<IPlanAdManager, PlanAdManager>();
            services.AddScoped<IPlanTimeManager, PlanTimeManager>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddCoreServices(this IServiceCollection services)
        {       
            services.AddScoped<IAdManager, AdManager>();
            services.AddScoped<IScheduleManager, ScheduleManager>();
            services.AddScoped<IPlanManager, PlanManager>();
            services.AddScoped<IPlanAdManager, PlanAdManager>();
            services.AddScoped<IUserService, UserService>();

            var scheduleValidators = Assembly.GetCallingAssembly().GetTypes()             
                        .Where(t => t.GetInterfaces().Any(i => i == typeof(IScheduleValidator)) && !t.IsAbstract);

            foreach (var scheduleValidator in scheduleValidators)
            {
                services.AddScoped(typeof(IScheduleValidator), scheduleValidator);
            }

            services.AddScoped<IScheduleValidatorFactory, ScheduleValidatorFactory>();
            services.AddScoped<IScheduleTimeServiceProvider, ScheduleTimeServiceProvider>();

            //todo: uncomment in Production
            //var awsConfig = new AWSS3Config();
            //configuration.Bind(nameof(AWSS3Config), awsConfig);
            //var awsCredentials = new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey);
            //var regionEndpoint = RegionEndpoint.GetBySystemName(awsConfig.RegionEndpointName);
            //var awsClient = new AmazonS3Client(awsCredentials, regionEndpoint);

            //services.AddScoped<IContentStorage>(p => new AWSS3Storage(awsClient, awsConfig.BucketName));

            services.AddScoped<IContentStorage, LocalStorage>();
            services.AddScoped<IContentServiceProvider, ContentServiceProvider>();
            services.AddScoped<IContentValidatorProvider, ContentValidatorProvider>();

            services.AddScoped<IReplicationHandlerFactory<AdPoint>, AdPointReplicationFactory>();
            services.AddScoped<IReplicationHandlerFactory<Tariff>, TariffReplicationFactory>();

            services.AddSingleton<IBasicConsumer, ReplicationConsumer<AdPoint>>();
            services.AddSingleton<IBasicConsumer, ReplicationConsumer<Tariff>>();
            services.AddSingleton<IBasicConsumer, PlanAcceptedConsumer>();

            services.AddAutoMapper(c =>
            {
                c.AddProfile<ScheduleProfile>();
                c.AddProfile<PlanProfile>();
                c.AddProfile<EventProfile>();
            });
        }
    }
}
