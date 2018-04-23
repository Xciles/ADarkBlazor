using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using System;
using System.Linq;
using ADarkBlazor.Services;
using ADarkBlazor.Services.Buttons;
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
                configure.AddScoped<IStoryService, StoryService>();

                configure.AddScoped<ApplicationState>();
                configure.AddScoped<IResourceService, ResourceService>();
                configure.AddScoped<IUserInputService, UserInputService>();
                configure.AddScoped<IVisibilityService, VisibilityService>();

                configure.AddScoped<IStory, StoryButton>();

                //var type = typeof(IButtonBase);
                //var types = AppDomain.CurrentDomain.GetAssemblies()
                //                        .SelectMany(s => s.GetTypes())
                //                        .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && p.IsInterface);


                //foreach (var type1 in types)
                //{
                //    configure.AddScoped(type1.GetInterfaces()[1], type1);
                //    //type1.inter
                //}
            });


            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
