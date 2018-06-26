using System;
using System.Threading;
using ADarkBlazor.Services.Buildings.Interfaces;
using ADarkBlazor.Services.Workers.Interfaces;

namespace ADarkBlazor.Services.Buildings
{
    public abstract class Building : IBuilding
    {
        private int _rawBuildTime;

        public event Action OnChange;
        public bool IsVisible { get; set; }
        public void NotifyStateChanged() => OnChange?.Invoke();
        public string Name { get; set; } = "Worker";
        public int NumberOfBuildings { get; set; } = 0;
        public bool IsUnlocked => NumberOfBuildings > 0;

        public virtual int BuildTime
        {
            get
            {
                if (Builder.NumberOfWorkers == 0) return _rawBuildTime / HyperState.DivideBy;
                return (_rawBuildTime / Builder.NumberOfWorkers) / HyperState.DivideBy;
            }
            set { if (!(_rawBuildTime.Equals(value))) _rawBuildTime = value; }
        }

        protected Timer BuildTimer { get; set; }
        protected IHyperState HyperState { get; }
        protected IBuilder Builder { get; set; }

        protected Building(IBuilder builder, IHyperState hyperState)
        {
            Builder = builder;
            HyperState = hyperState;
        }

        public void Enable()
        {
            IsVisible = true;
            NotifyStateChanged();
        }

        public abstract void Build();
        public abstract void Demolish();

        // Old implementation, not sure if this is handy?
        //public virtual void Build()
        //{
        //    if (CallbackImplementation())
        //    {
        //        NumberOfBuildings++;

        //        NotifyStateChanged();
        //    }
        //}

        //public virtual void Demolish()
        //{
        //    NumberOfBuildings--;

        //    NotifyStateChanged();
        //}
        //protected abstract bool CallbackImplementation();
    }
}