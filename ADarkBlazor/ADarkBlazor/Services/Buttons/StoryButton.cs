using System.Threading;
using ADarkBlazor.Exceptions;
using ADarkBlazor.Services.Buttons.Interfaces;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources;
using ADarkBlazor.Services.Resources.Interfaces;

namespace ADarkBlazor.Services.Buttons
{
    public class StoryButton : ButtonBase, IStory
    {
        private readonly IStoryService _storyService;
        private readonly IWood _wood;
        private int _numberOfClicks;

        public StoryButton(ApplicationState state, IStoryService storyService, IWood wood) : base(state)
        {
            _storyService = storyService;
            _wood = wood;
            IsVisible = true;
            IsClickable = true;
            Title = "search wood";
            Cooldown = 5_000;
        }

        public override void InvokeImplementation()
        {
            try
            {
                if (_numberOfClicks == 1)
                {
                    Title = "light fire";
                    _numberOfClicks++;
                }
                else if (_numberOfClicks > 2)
                {
                    Title = "stoke fire";
                    _wood.Subtract(1);
                }

                _numberOfClicks++;

                _storyService.Invoke(EStoryEventType.StoryEvent);
            }
            catch (ResourceException)
            {
                _storyService.Invoke(EStoryEventType.NoMoreWoodEvent);
            }
        }

        public override void TimerFinished()
        {
        }
    }
}