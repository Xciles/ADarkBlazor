using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services
{
    public class ApplicationState
    {
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        private Timer _timer;
        private Timer _unlockTimer;

        public IList<OutputInfo> PrintedInformation { get; private set; } = new List<OutputInfo>();
        public IVisibilityService Visibility { get; set; }
        public bool IsTrue { get; set; } = false;

        public ApplicationState(IVisibilityService visibility)
        {
            Visibility = visibility;
            _timer = new Timer(TimerCallback, null, 1_000, 1_000);
        }

        private void TimerCallback(object state)
        {
            AddSomethingToInformation($"Just some info: {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {PrintedInformation.Count}");
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
            IsTrue = true;
            _unlockTimer?.Dispose();
            _unlockTimer = new Timer(state =>
            {
                Visibility.Unlock(EMenuType.Test);
                IsTrue = false;
                NotifyStateChanged();
            }, null, 1_000, -1);
        }
    }
}
