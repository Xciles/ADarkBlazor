using ADarkBlazor.Services.Buildings.Interfaces;
using ADarkBlazor.Services.Buttons.Interfaces;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services.Buttons
{
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