using System;
using System.Collections.Generic;
using System.Linq;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources;
using ADarkBlazor.Services.Resources.Interfaces;

namespace ADarkBlazor.Services
{
    public class ResourceService : IResourceService
    {
        private ApplicationState _state;
        public event Action OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();
        public IList<IResource> Resources { get; set; } = new List<IResource>();

        public ResourceService(ApplicationState state)
        {
            _state = state;
        }

        public void RegisterResources(IServiceProvider provider)
        {
            var type = typeof(IResource);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsInterface && p.GetInterfaces().Contains(type));

            foreach (var type1 in types)
            {
                Resources.Add((IResource)provider.GetService(type1));
            }
        }

        public void EnableResource(EResourceType type)
        {
            Resources.First(x => x.ResourceType == type).Enable();
        }

        public void AddToResource(EResourceType type, double amount)
        {
            Resources.First(x => x.ResourceType == type).Add(amount);
        }

        public void SubtractFromResource(EResourceType type, double amount)
        {
            Resources.First(x => x.ResourceType == type).Subtract(amount);
        }
    }
}