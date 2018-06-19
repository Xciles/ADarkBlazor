using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources;

namespace ADarkBlazor.Services.Workers
{
    public interface IIdleWorker : IWorker
    {

    }
    public interface IWoodGatherer : IWorker
    {

    }
    public interface IFisherman : IWorker
    {

    }

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

    public class IdleWorker : Worker, IIdleWorker
    {
        public IdleWorker()
        {
            Name = "Idle Worker";
            //NumberOfWorkers = 4;
        }
    }

    public interface IBuilder : IWorker
    {

    }


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

    public class Builder : Crafter, IBuilder
    {
        public Builder(IFood food, IHyperState hyperState) : base(food, hyperState)
        {
        }
    }

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

    public class WoodGatherer : Gatherer, IWoodGatherer
    {
        public WoodGatherer(IWood wood, IHyperState hyperState) : base(hyperState, wood)
        {
            Name = "Wood Gatherer";
        }

        protected override void CallbackImplementation()
        {
        }
    }

    public class Fisherman : Gatherer, IFisherman
    {
        public Fisherman(IFood food, IHyperState hyperState) : base(hyperState, food)
        {
            Name = "Fisherman";
            AmountPer10Seconds = 10;
        }

        protected override void CallbackImplementation()
        {
        }
    }
}
