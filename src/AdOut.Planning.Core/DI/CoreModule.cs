﻿using AdOut.Planning.Core.Content.Factories;
using AdOut.Planning.Core.Content.Storages;
using AdOut.Planning.Core.Managers;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace AdOut.Planning.Core.DI
{
    public static class CoreModule
    {
        public static void AddCoreModule (this IServiceCollection services)
        {
            services.AddScoped<IAdManager, AdManager>();

            services.AddScoped<IDirectoryDistributor, EmptyDirectoryDistributor>();
            services.AddScoped<IContentStorage, LocalStorage>();
            services.AddScoped<IContentComponentsProvider, ContentComponentsProvider>();
        }
    }
}
