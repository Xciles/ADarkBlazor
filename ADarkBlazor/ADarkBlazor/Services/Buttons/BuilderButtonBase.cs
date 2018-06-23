using System;
using System.Threading;
using ADarkBlazor.Exceptions;
using ADarkBlazor.Services.Buildings;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services.Buttons
{
    public abstract class BuilderButtonBase : ButtonBase, IBuilderButtonBase
    {
        protected IBuilding Building { get; }
        private Timer _timer;
        private const int Interval = 100;

        protected IStoryService StoryService { get; }

        protected BuilderButtonBase(ApplicationState state, IStoryService storyService, IBuilding building) : base (state)
        {
            Building = building;
            StoryService = storyService;
        }

        public override void Invoke()
        {
            if (IsClickable)
            {
                IsClickable = false;
                Cooldown = Building.BuildTime;
                RemainingCooldown = Cooldown;

                InvokeImplementation();

                NotifyStateChanged();

                _timer?.Dispose();
                _timer = new Timer(ButtonBuildingCallback, null, 0, Interval);
            }
        }

        private void ButtonBuildingCallback(object state)
        {
            RemainingCooldown -= Interval;
            if (RemainingCooldown <= 0)
            {
                _timer?.Dispose();

                IsClickable = true;

                TimerFinished();
                NotifyStateChanged();
            }
        }

        public override void InvokeImplementation()
        {
            try
            {
                Building.Build();
            }
            catch (ResourceException ex)
            {
                StoryService.Invoke(ex.Message);
            }
            catch (BuilderException ex)
            {
                StoryService.Invoke(ex.Message);
            }
        }

        // Just implement the timerfinished here. So we don't have to implement this in all classes
        public override void TimerFinished() { }
    }

    public interface IBuildHouse : IBuilderButtonBase
    {

    }

    public class BuildHouse : BuilderButtonBase, IBuildHouse
    {
        public BuildHouse(ApplicationState state, IStoryService storyService, IHouse house) : base(state, storyService, house)
        {
            IsVisible = true;
            IsClickable = true;
            Title = "Build House";
        }
    }

    public interface IBuildTownHall : IBuilderButtonBase
    {

    }

    public class BuildTownHall : BuilderButtonBase, IBuildTownHall
    {
        public BuildTownHall(ApplicationState state, IStoryService storyService, ITownHall townHall) : base(state, storyService, townHall)
        {
            IsVisible = true;
            IsClickable = true;
            Title = "Build Town Hall";
        }

        public override void TimerFinished()
        {
            base.TimerFinished();

            if (Building.NumberOfBuildings > 0)
            {
                IsClickable = false;
            }
        }
    }
}
