using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using System;
using System.Linq;
using ADarkBlazor.Services;
using ADarkBlazor.Services.Buildings;
using ADarkBlazor.Services.Buttons;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources;
using ADarkBlazor.Services.Workers;
using Blazor.Extensions.Storage;
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
                configure.AddScoped<IResourceService, ResourceService>();
                configure.AddScoped<IWorkerService, WorkerService>();
                configure.AddScoped<IStoryService, StoryService>();
                configure.AddScoped<IUserInputService, UserInputService>();
                configure.AddScoped<IVisibilityService, VisibilityService>();
                configure.AddScoped<ISaveStateService, SaveStateService>();

                configure.AddScoped<IStory, StoryButton>();
                configure.AddScoped<IGatherWood, GatherWoodButton>();

                configure.AddScoped<IWood, Wood>();
                configure.AddScoped<IFood, Food>();

                configure.AddScoped<IIdleWorker, IdleWorker>();
                configure.AddScoped<IWoodGatherer, WoodGatherer>();
                configure.AddScoped<IFisherman, Fisherman>();
                configure.AddScoped<IBuilder, Builder>();

                configure.AddScoped<IBuildHouse, BuildHouse>();
                configure.AddScoped<IBuildTownHall, BuildTownHall>();

                configure.AddScoped<ITownHall, TownHall>();
                configure.AddScoped<IHouse, House>();

                configure.AddScoped<IHyper, HyperButton>();
                configure.AddScoped<IHyperState, HyperState>();

                configure.AddStorage();
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
