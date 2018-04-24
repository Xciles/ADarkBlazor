using System.Threading;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services.Buttons
{
    public class StoryButton : ButtonBase, IStory
    {
        private readonly IStoryService _storyService;
        private Timer _timer;

        public StoryButton(ApplicationState state, IStoryService storyService) : base(state)
        {
            _storyService = storyService;
            IsVisible = true;
            IsClickable = true;
            Title = "search wood";
        }

        public override void Invoke()
        {
            if (IsClickable)
            {
                IsClickable = false;
                Title = "stoke fire";

                _storyService.Invoke(EStoryEventType.StoryEvent);

                NotifyStateChanged();
                _timer?.Dispose();
                _timer = new Timer(Callback, null, 4_000, -1);
            }
        }

        private void Callback(object state)
        {
            IsClickable = true;
            NotifyStateChanged();
        }
    }
}