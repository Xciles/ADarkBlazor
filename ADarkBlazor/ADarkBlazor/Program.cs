using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using System;
using ADarkBlazor.Services;
using ADarkBlazor.Services.Interfaces;
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
                configure.AddScoped<IButtonClickableService, ButtonClickableService>();
                configure.AddScoped<IButtonVisibilityService, ButtonVisibilityService>();
                configure.AddScoped<IResourceService, ResourceService>();
                configure.AddScoped<IUserInputService, UserInputService>();
                configure.AddScoped<IVisibilityService, VisibilityService>();
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
