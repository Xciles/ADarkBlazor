using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services.Buttons
{
    public class ResetButton : ButtonBase, IReset
    {
        private readonly IStoryService _storyService;
        private readonly ISaveStateService _saveStateService;

        public ResetButton(ApplicationState state, IStoryService storyService, ISaveStateService saveStateService) : base(state)
        {
            _storyService = storyService;
            _saveStateService = saveStateService;

            Title = "Reset";
            IsVisible = true;
            IsClickable = true;
        }

        public override void InvokeImplementation()
        {
            _saveStateService.Reset();

            _storyService.Invoke(EStoryEventType.Reset);
        }

        public override void TimerFinished()
        {
        }
    }
}