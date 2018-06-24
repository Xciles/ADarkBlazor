using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ADarkBlazor.Services.Buttons;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Interfaces;
using Blazor.Extensions;
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
        private readonly LocalStorage _localStorage;
        private readonly ISaveStateService _saveStateService;
        public event Action OnChange;
        public IHyperState HyperState { get; set; }
        public void NotifyStateChanged() => OnChange?.Invoke();
        private Timer _saveStateTimer;
        private IList<IButtonBase> _buttons = new List<IButtonBase>();

        public ApplicationState(IServiceProvider provider, IHyperState hyperState, LocalStorage localStorage, ISaveStateService saveStateService)
        {
            _provider = provider;
            _localStorage = localStorage;
            _saveStateService = saveStateService;

            HyperState = hyperState;

            _saveStateTimer = new Timer(SaveStateTimerCallback, null, 1_000, 60 * 1_000);
            //_saveStateTimer = new Timer(SaveStateTimerCallback, null, 60 * 1_000, 60 * 1_000);
            ReadState();
        }

        private void SaveStateTimerCallback(object state)
        {
            SaveState();
            _saveStateService.Save();
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
            _localStorage.SetItem("AppState", new SaveState { Message = $"Saved info: {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {_buttons.Count}" });
        }

        private void ReadState()
        {
            var message = _localStorage.GetItem<SaveState>("AppState");
        }

        public string Test()
        {
            return $"{DateTime.Now:HH:mm:ss}";
        }
    }

    public interface IHasSaveState
    {
        void Save(SaveState state);
        void Load(SaveState state);
    }

    public interface ISaveStateService
    {
        void Save();
    }

    public class SaveStateService : ISaveStateService
    {
        private readonly IServiceProvider _provider;
        private readonly LocalStorage _localStorage;

        public SaveStateService(IServiceProvider provider, LocalStorage localStorage)
        {
            _provider = provider;
            _localStorage = localStorage;
        }

        public void Save()
        {
            var state = new SaveState();

            var type = typeof(IHasSaveState);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsInterface && p.GetInterfaces().Contains(type));

            Console.WriteLine($"number of {types.Count()}");

            foreach (var type1 in types)
            {
                Console.WriteLine($"type: {type1.Name}");
                var instance = (IHasSaveState) _provider.GetService(type1);

                instance?.Save(state);
            }

            state.Time = DateTime.UtcNow;
            _localStorage.SetItem("SaveState", state);
        }
    }
}
