using System;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources;
using ADarkBlazor.Services.Workers.Interfaces;

namespace ADarkBlazor.Services.Workers
{
    public abstract class Worker : IWorker
    {
        public event Action OnChange;
        public bool IsVisible { get; set; }
        public void NotifyStateChanged() => OnChange?.Invoke();
        public string Name { get; set; } = "Worker";
        public int NumberOfWorkers { get; set; } = 0;
        public bool IsUnlocked { get; set; }

        public void AddWorker()
        {
            NumberOfWorkers++;
            IsUnlocked = true;

            NotifyStateChanged();
        }

        public void SubtractWorker()
        {
            NumberOfWorkers--;

            NotifyStateChanged();
        }

        public void Enable()
        {
            IsVisible = true;
            NotifyStateChanged();
        }
    }
}
