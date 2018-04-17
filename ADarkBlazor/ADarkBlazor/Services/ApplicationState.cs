using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using ADarkBlazor.Services.Buttons;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ADarkBlazor.Services
{
    public class ApplicationState
    {
        private readonly IServiceProvider _provider;
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        private Timer _timer;
        private Timer _unlockTimer;

        public IList<OutputInfo> PrintedInformation { get; private set; } = new List<OutputInfo>();
        public IVisibilityService Visibility { get; set; }
        private IList<IButtonBase> _buttons = new List<IButtonBase>();

        public ApplicationState(IVisibilityService visibility, IServiceProvider provider)
        {
            _provider = provider;
            Visibility = visibility;
            _timer = new Timer(TimerCallback, null, 1_000, 1_000);
        }

        private void TimerCallback(object state)
        {
            AddSomethingToInformation($"Just some info: {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {PrintedInformation.Count} - {_buttons.Count}");
        }

        public void RegisterButtons()
        {
            var type = typeof(IButtonBase);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(s => s.GetTypes())
                                    .Where(p => p.IsInterface && p.GetInterfaces().Contains(type));

                AddSomethingToInformation($"Just some info: ");
            foreach (var type1 in types)
            {
                AddSomethingToInformation($"Just some info: {type1.Name}");
                _buttons.Add((IButtonBase)_provider.GetService(type1));
            }


            //_buttons = _provider.GetServices<IButtonBase>().ToList();
        }

        private void AddSomethingToInformation(string strToAdd)
        {
            PrintedInformation.Insert(0, new OutputInfo { Info = strToAdd });

            if (PrintedInformation.Count > 30) PrintedInformation.RemoveAt(30);

            NotifyStateChanged();
        }

        public void AddRoomInfo()
        {
            AddSomethingToInformation($"Add thing");
            _unlockTimer?.Dispose();
            _unlockTimer = new Timer(state =>
            {
                Visibility.Unlock(EMenuType.Test);
            }, null, 1_000, -1);
        }
    }
}
