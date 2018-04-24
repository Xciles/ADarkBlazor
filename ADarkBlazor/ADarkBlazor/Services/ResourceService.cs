using System;
using System.Collections.Generic;
using System.Linq;
using ADarkBlazor.Services.Buttons;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources;

namespace ADarkBlazor.Services
{
    public class ResourceService : IResourceService
    {
        private IList<IResource> _resources = new List<IResource>();



        public void RegisterReources(IServiceProvider provider)
        {
            var type = typeof(IResource);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsInterface && p.GetInterfaces().Contains(type));

            foreach (var type1 in types)
            {
                _resources.Add((IResource)provider.GetService(type1));
            }
        }
    }
}