using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ADarkBlazor.Services.Buttons;
using ADarkBlazor.Services.Interfaces;
using BlazorExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace ADarkBlazor.Services
{
    public class ApplicationState
    {
        private bool _isInitialized = false;
        private readonly IServiceProvider _provider;
        public event Action OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();
        private Timer _saveStateTimer;

        private IList<IButtonBase> _buttons = new List<IButtonBase>();

        public ApplicationState(IServiceProvider provider)
        {
            _provider = provider;
            _saveStateTimer = new Timer(SaveStateTimerCallback, null, 60 * 1_000, 60 * 1_000);
            ReadState();
        }

        private void SaveStateTimerCallback(object state)
        {
            SaveState();
        }

        public void RegisterButtons()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                var type = typeof(IButtonBase);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                                        .SelectMany(s => s.GetTypes())
                                        .Where(p => p.IsInterface && p.GetInterfaces().Contains(type));

                foreach (var type1 in types)
                {
                    _buttons.Add((IButtonBase)_provider.GetService(type1));
                }

                _provider.GetService<IResourceService>().RegisterResources(_provider);
                _provider.GetService<IWorkerService>().RegisterWorkers(_provider);
            }
        }

        private void SaveState()
        {
            Browser.WriteStorage("AppState", $"Saved info: {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {_buttons.Count}");
        }

        private void ReadState()
        {
            var str = Browser.ReadStorage("AppState");
        }

        public string Test()
        {
            return $"{DateTime.Now:HH:mm:ss}";
        }
    }
}
