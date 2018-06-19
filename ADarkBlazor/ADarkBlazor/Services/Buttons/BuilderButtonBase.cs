using System;
using System.Threading;
using ADarkBlazor.Exceptions;
using ADarkBlazor.Services.Buildings;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services.Buttons
{
    public abstract class BuilderButtonBase : IBuilderButtonBase
    {
        protected IBuilding Building { get; }
        public event Action OnChange;
        private Timer _timer;
        private const int Interval = 100;
        private int _cooldown;

        protected void NotifyStateChanged()
        {
            OnChange?.Invoke();
            State.NotifyStateChanged();
        }

        protected ApplicationState State { get; }
        protected IStoryService StoryService { get; }
        public bool IsVisible { get; set; }
        public bool IsClickable { get; set; }
        public virtual EButtonType ButtonType { get; set; }
        public string Title { get; set; }
        public int RemainingCooldown { get; set; }
        public int Cooldown
        {
            get => _cooldown / State.HyperState.DivideBy;
            set { if (!(_cooldown.Equals(value))) _cooldown = value; }
        }

        protected BuilderButtonBase(ApplicationState state, IStoryService storyService, IBuilding building)
        {
            Building = building;
            State = state;
            StoryService = storyService;
        }

        public void Invoke()
        {
            if (IsClickable)
            {
                IsClickable = false;
                Cooldown = Building.BuildTime;
                RemainingCooldown = Cooldown;

                InvokeImplementation();

                NotifyStateChanged();

                _timer?.Dispose();
                _timer = new Timer(Callback, null, 0, Interval);
            }
        }

        private void Callback(object state)
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

        public virtual void InvokeImplementation()
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

        public virtual void TimerFinished() { }
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
