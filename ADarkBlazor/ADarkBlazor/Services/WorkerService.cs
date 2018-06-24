using System;
using System.Collections.Generic;
using System.Linq;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources;
using ADarkBlazor.Services.Workers;
using ADarkBlazor.Services.Workers.Interfaces;

namespace ADarkBlazor.Services
{
    public class WorkerService : IWorkerService
    {
        private ApplicationState _state;
        public event Action OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();
        public IList<IWorker> Workers { get; set; } = new List<IWorker>();

        public void EnableWorker(Type workerType)
        {
            Workers.First(x => x.GetType() == workerType).Enable();
        }

        public void AddPersonToWorker(Type workerType) 
        {
            Workers.First(x => x.GetType() == workerType).AddWorker();
            if (workerType != typeof(IdleWorker))
            {
                Workers.First(x => x.GetType() == typeof(IdleWorker)).SubtractWorker();
            }
        }

        public void SubtractPersonFromWorker(Type workerType)
        {
            Workers.First(x => x.GetType() == workerType).SubtractWorker();
            if (workerType != typeof(IdleWorker))
            {
                Workers.First(x => x.GetType() == typeof(IdleWorker)).AddWorker();
            }
        }

        public int TotalInhabitants()
        {
            return Workers.Sum(x => x.NumberOfWorkers);
        }

        public WorkerService(ApplicationState state)
        {
            _state = state;
        }

        // Some kind of thing that will insance every person into its own worker
        // instead of keeping the number of workers in some field in the worker.
        //public IDictionary<IWorker, int> DistinctWorkers
        //{
        //    get
        //    {
        //        return Workers.GroupBy(x => x).ToDictionary(x => x.Key, g => g.Count());
        //    }
        //}

        public void RegisterWorkers(IServiceProvider provider)
        {
            var type = typeof(IWorker);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsInterface && p.GetInterfaces().Contains(type));

            foreach (var type1 in types)
            {
                Workers.Add((IWorker)provider.GetService(type1));
            }
        }
    }
}