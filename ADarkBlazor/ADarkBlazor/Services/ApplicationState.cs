using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ADarkBlazor.Services.Buttons;
using ADarkBlazor.Services.Buttons.Interfaces;
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
        private readonly ISaveStateService _saveStateService;
        public event Action OnChange;
        public IHyperState HyperState { get; set; }
        public void NotifyStateChanged() => OnChange?.Invoke();
        private Timer _saveStateTimer;
        private IList<IButtonBase> _buttons = new List<IButtonBase>();

        public ApplicationState(IServiceProvider provider, IHyperState hyperState, ISaveStateService saveStateService)
        {
            _provider = provider;
            _saveStateService = saveStateService;

            HyperState = hyperState;

            _saveStateTimer = new Timer(SaveStateTimerCallback, null, 60 * 1_000, 60 * 1_000);
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

                ReadState();
            }
        }

        private void SaveState()
        {
            _saveStateService.Save();
        }

        private void ReadState()
        {
            _saveStateService.Load();
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
        void Reset();
    }

    public interface ISaveStateService
    {
        void Save();
        void Load();
        void Reset();
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
            var types = GetAllSaveTypes();
            var state = new SaveState();

            foreach (var type1 in types)
            {
                var instance = (IHasSaveState) _provider.GetService(type1);

                instance?.Save(state);
            }

            state.Time = DateTime.UtcNow;
            _localStorage.SetItem("SaveState", state);
        }

        public void Load()
        {
            var types = GetAllSaveTypes();
            var state = _localStorage.GetItem<SaveState>(nameof(SaveState));
            if (state == null) return; // No save state available

            Console.WriteLine($"{state.Time}");
            foreach (var type in types)
            {
                var instance = (IHasSaveState) _provider.GetService(type);
                instance?.Load(state);
            }
        }

        public void Reset()
        {
            var types = GetAllSaveTypes();

            foreach (var type in types)
            {
                var instance = (IHasSaveState) _provider.GetService(type);
                instance?.Reset();
            }

            _localStorage.Clear();
        }

        private static IEnumerable<Type> GetAllSaveTypes()
        {
            var type = typeof(IHasSaveState);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsInterface && p.GetInterfaces().Contains(type));
            return types;
        }
    }
}
