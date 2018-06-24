using System;
using System.Threading;
using ADarkBlazor.Exceptions;
using ADarkBlazor.Services.Buildings.Interfaces;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources.Interfaces;
using ADarkBlazor.Services.Workers.Interfaces;

namespace ADarkBlazor.Services.Buildings
{
    public class TownHall : Building, ITownHall
    {
        private readonly IWood _wood;
        private readonly IBuilder _builder;
        private readonly IVisibilityService _visibilityService;
        private readonly IStoryService _storyService;
        private const int _woodRequired = 105;
        
        public TownHall(IWood wood, IBuilder builder, IVisibilityService visibilityService, IStoryService storyService, IHyperState hyperState, IBuilder builder1) : base(builder1, hyperState)
        {
            _wood = wood;
            _builder = builder;
            _visibilityService = visibilityService;
            _storyService = storyService;

            BuildTime = 50_000;
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
            BuildTimer = new Timer(BuildingFinished, null, (BuildTime / HyperState.DivideBy) - 10, -1);
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