using System;
using System.Threading;
using ADarkBlazor.Exceptions;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources;
using ADarkBlazor.Services.Workers;

namespace ADarkBlazor.Services.Buildings
{
    public interface IBuilding
    {

        event Action OnChange;
        bool IsVisible { get; set; }
        string Name { get; set; }
        int NumberOfBuildings { get; set; }
        bool IsUnlocked { get; }
        int BuildTime { get; }

        void Build();
        void Demolish();

        void Enable();
    }

    public abstract class Building : IBuilding
    {
        public event Action OnChange;
        public bool IsVisible { get; set; }
        public void NotifyStateChanged() => OnChange?.Invoke();
        public string Name { get; set; } = "Worker";
        public int NumberOfBuildings { get; set; } = 0;
        public bool IsUnlocked => NumberOfBuildings > 0;
        public virtual int BuildTime { get; } = 0; // todo expose raw time
        protected Timer BuildTimer { get; set; }
        protected IHyperState HyperState { get; }

        protected Building(IHyperState hyperState)
        {
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

    public interface IHouse : IBuilding
    {

    }

    public class House : Building, IHouse
    {
        private readonly IWood _wood;
        private readonly IBuilder _builder;
        private readonly ITownHall _townHall;
        private readonly IWorkerService _workerService;
        private readonly IStoryService _storyService;
        private const int _woodRequired = 45;
        private const int _rawBuildTime = 30_000;
        private const int _numberOfInhabitantsPerHouse = 5;
        private Timer _timer;

        public override int BuildTime
        {
            get
            {
                if (_builder.NumberOfWorkers == 0) return _rawBuildTime / HyperState.DivideBy;
                return (_rawBuildTime / _builder.NumberOfWorkers) / HyperState.DivideBy;
            }
        }

        public House(IWood wood, IBuilder builder, ITownHall townHall, IWorkerService workerService, IStoryService storyService, IHyperState hyperState) : base(hyperState)
        {
            _wood = wood;
            _builder = builder;
            _townHall = townHall;
            _workerService = workerService;
            _storyService = storyService;

            HyperState.OnChange += HyperStateOnOnChange;
            _timer = new Timer(InhabitantsCallback, null, 30_000 / HyperState.DivideBy, 30_000 / HyperState.DivideBy);
        }

        private void HyperStateOnOnChange()
        {
            _timer.Dispose();
            _timer = null;
            _timer = new Timer(InhabitantsCallback, null, 30_000 / HyperState.DivideBy, 30_000 / HyperState.DivideBy);
        }

        private void InhabitantsCallback(object state)
        {
            var totalInhabitants = _workerService.TotalInhabitants();
            var diff = (NumberOfBuildings * _numberOfInhabitantsPerHouse) - totalInhabitants;

            if (diff > 0)
            {
                var rand = new Random();
                var inhabitantsToAdd = rand.Next(2, 5);
                inhabitantsToAdd = inhabitantsToAdd > diff ? diff : inhabitantsToAdd;

                for (int i = 0; i < inhabitantsToAdd; i++)
                {
                    _workerService.AddPersonToWorker(typeof(IdleWorker));
                }

                    _storyService.Invoke($"A new Family of {inhabitantsToAdd} took refuge in your town!");
                if (!_townHall.IsUnlocked)
                {
                    for (int i = 0; i < inhabitantsToAdd - 1; i++)
                    {
                        _workerService.AddPersonToWorker(typeof(WoodGatherer));
                    }

                    _storyService.Invoke($"A few of them became Wood Gatherers");
                }
            }
        }

        public override void Build()
        {
            if (_builder.NumberOfWorkers <= 0)
            {
                throw new BuilderException("No builder selected, you cannot build...");
            }

            if (_wood.Amount < _woodRequired)
            {
                throw new ResourceException("Not enough wood, please get more wood...");
            }

            _wood.Subtract(_woodRequired);
            BuildTimer = new Timer(BuildingFinished, null, BuildTime - 10, -1);
        }

        private void BuildingFinished(object state)
        {
            NumberOfBuildings++;
            NotifyStateChanged();
        }

        public override void Demolish()
        {
            throw new NotImplementedException();
        }
    }

    public interface ITownHall : IBuilding
    {

    }

    public class TownHall : Building, ITownHall
    {
        private readonly IWood _wood;
        private readonly IBuilder _builder;
        private readonly IVisibilityService _visibilityService;
        private readonly IStoryService _storyService;
        private const int _woodRequired = 105;
        private const int _rawBuildTime = 50_000;

        public override int BuildTime
        {
            get
            {
                if (_builder.NumberOfWorkers == 0) return _rawBuildTime / HyperState.DivideBy;
                return (_rawBuildTime / _builder.NumberOfWorkers) / HyperState.DivideBy;
            }
        }

        public TownHall(IWood wood, IBuilder builder, IVisibilityService visibilityService, IStoryService storyService, IHyperState hyperState) : base(hyperState)
        {
            _wood = wood;
            _builder = builder;
            _visibilityService = visibilityService;
            _storyService = storyService;
        }

        public override void Build()
        {
            if (_builder.NumberOfWorkers <= 0)
            {
                throw new BuilderException("No builder selected, you cannot build...");
            }

            if (_wood.Amount < _woodRequired)
            {
                throw new ResourceException("Not enough wood, please get more wood...");
            }

            _wood.Subtract(_woodRequired);
            _storyService.Invoke($"Building the Town Hall...");
            BuildTimer = new Timer(BuildingFinished, null, BuildTime - 10, -1);
        }

        private void BuildingFinished(object state)
        {
            NumberOfBuildings++;

            _visibilityService.Unlock(EMenuType.TownHall);

            _storyService.Invoke($"The Town Hall was build!");

            NotifyStateChanged();
        }

        public override void Demolish()
        {
            throw new NotImplementedException();
        }
    }
}
