﻿using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using System;
using ADarkBlazor.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ADarkBlazor
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(configure =>
            {
                // Add any custom services here
                configure.AddScoped<ApplicationState>();
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
