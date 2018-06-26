using System.Threading;
using ADarkBlazor.Services.Resources.Interfaces;

namespace ADarkBlazor.Services.Workers
{
    public abstract class Crafter : Worker
    {
        private readonly IHyperState _hyperState;
        protected double Upkeep { get; set; } = 1;
        protected IFood Food { get; }
        private Timer _timer;

        protected Crafter(IFood food, IHyperState hyperState)
        {
            _hyperState = hyperState;
            _hyperState.OnChange += HyperStateOnOnChange;
            Food = food;

            _timer = new Timer(Callback, null, 10_000 / _hyperState.DivideBy, 10_000 / _hyperState.DivideBy);
        }

        private void HyperStateOnOnChange()
        {
            _timer.Dispose();
            _timer = null;
            _timer = new Timer(Callback, null, 10_000 / _hyperState.DivideBy, 10_000 / _hyperState.DivideBy);
        }

        private void Callback(object state)
        {
            // just upkeep
            Food.Subtract(Upkeep * NumberOfWorkers);
        }
    }
}