using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ADarkBlazor.Services.Domain;

namespace ADarkBlazor.Services
{
    public class ApplicationState
    {
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public IList<OutputInfo> PrintedInformation { get; private set; } = new List<OutputInfo>();
        private Timer _timer;

        public ApplicationState()
        {
            _timer = new Timer(TimerCallback, null, 1_000, 1_000);
        }

        private void TimerCallback(object state)
        {
            PrintedInformation.Add(new OutputInfo { Info = $"Just some info: {DateTime.Now:yyyy-MM-dd HH:mm:ss}"});
            NotifyStateChanged();
        }
    }
}
