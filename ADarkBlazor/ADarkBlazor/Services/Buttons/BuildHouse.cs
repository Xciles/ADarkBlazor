using ADarkBlazor.Services.Buildings.Interfaces;
using ADarkBlazor.Services.Buttons.Interfaces;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services.Buttons
{
    public class BuildHouse : BuilderButtonBase, IBuildHouse
    {
        public BuildHouse(ApplicationState state, IStoryService storyService, IHouse house) : base(state, storyService, house)
        {
            IsVisible = true;
            IsClickable = true;
            Title = "Build House";
        }
    }
}