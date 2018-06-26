using System;

namespace ADarkBlazor.Services.Workers.Interfaces
{
    public interface IWorker
    {
        event Action OnChange;

        bool IsVisible { get; set; }
        string Name { get; set; }
        int NumberOfWorkers { get; set; }
        bool IsUnlocked { get; set; }

        void AddWorker();
        void SubtractWorker();

        void Enable();
    }
}