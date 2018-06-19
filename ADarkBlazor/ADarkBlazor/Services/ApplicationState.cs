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
    public interface IHyperState
    {
        event Action OnChange;
        bool Enabled { get; set; }
        int DivideBy { get; set; }
    }

    public class HyperState : IHyperState
    {
        private int _divideBy = 5;
        private bool _enabled;

        public bool Enabled
        {
            get => _enabled;
            set { _enabled = value; NotifyStateChanged(); }
        }

        public int DivideBy
        {
            get => Enabled ? _divideBy : 1;
            set { _divideBy = value; NotifyStateChanged(); }
        }

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public class ApplicationState
    {
        private bool _isInitialized = false;
        private readonly IServiceProvider _provider;
        public event Action OnChange;
        public IHyperState HyperState { get; set; }
        public void NotifyStateChanged() => OnChange?.Invoke();
        private Timer _saveStateTimer;
        private IList<IButtonBase> _buttons = new List<IButtonBase>();

        public ApplicationState(IServiceProvider provider, IHyperState hyperState)
        {
            _provider = provider;
            HyperState = hyperState;
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
