using System;
using System.Threading;
using ADarkBlazor.Exceptions;
using ADarkBlazor.Services.Buildings.Interfaces;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources.Interfaces;
using ADarkBlazor.Services.Workers;
using ADarkBlazor.Services.Workers.Interfaces;

namespace ADarkBlazor.Services.Buildings
{
    public class House : Building, IHouse
    {
        private readonly IWood _wood;
        private readonly IBuilder _builder;
        private readonly ITownHall _townHall;
        private readonly IWorkerService _workerService;
        private readonly IStoryService _storyService;
        private const int _woodRequired = 45;
        private const int _numberOfInhabitantsPerHouse = 5;
        private Timer _timer;

        public House(IWood wood, IBuilder builder, ITownHall townHall, IWorkerService workerService, IStoryService storyService, IHyperState hyperState) : base(builder, hyperState)
        {
            _wood = wood;
            _builder = builder;
            _townHall = townHall;
            _workerService = workerService;
            _storyService = storyService;

            BuildTime = 30_000;

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
            BuildTimer = new Timer(BuildingFinished, null, (BuildTime / HyperState.DivideBy) - 10, -1);
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
}