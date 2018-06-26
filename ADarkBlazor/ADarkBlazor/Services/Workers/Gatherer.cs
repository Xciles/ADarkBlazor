using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ADarkBlazor.Services.Resources.Interfaces;

namespace ADarkBlazor.Services.Workers
{
    public abstract class Gatherer : Worker
    {
        private readonly IHyperState _hyperState;
        protected IList<IResource> Resources { get; private set; } = new List<IResource>();

        // amount per 10 seconds
        // levels
        public double AmountPer10Seconds { get; set; } = 1;
        private Timer _timer;

        protected Gatherer(IHyperState hyperState, params IResource[] resources)
        {
            _hyperState = hyperState;
            _hyperState.OnChange += HyperStateOnOnChange;

            foreach (var resource in resources)
            {
                Resources.Add(resource);
            }

            _timer = new Timer(Callback, null, 10_000 / hyperState.DivideBy, 10_000 / hyperState.DivideBy);
        }

        private void HyperStateOnOnChange()
        {
            _timer.Dispose();
            _timer = null;
            _timer = new Timer(Callback, null, 10_000 / _hyperState.DivideBy, 10_000 / _hyperState.DivideBy);
        }

        private void Callback(object state)
        {
            CallbackImplementation();

            if (Resources.Count == 1)
            {
                Resources.First().Add(AmountPer10Seconds * NumberOfWorkers);
            }
        }

        protected abstract void CallbackImplementation();
    }
}