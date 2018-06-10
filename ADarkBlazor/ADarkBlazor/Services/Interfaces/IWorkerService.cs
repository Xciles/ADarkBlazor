using System;
using System.Collections.Generic;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Workers;

namespace ADarkBlazor.Services.Interfaces
{
    public interface IWorkerService
    {
        event Action OnChange;
        IList<IWorker> Workers { get; set; }
        void RegisterWorkers(IServiceProvider provider);

        void EnableWorker(Type workerType);
        void AddPersonToWorker(Type workerType);
        void SubtractPersonFromWorker(Type workerType);

        int TotalInhabitants();
    }
}