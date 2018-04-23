using System;
using System.Threading;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services.Buttons
{
    public interface IButtonBase
    {
        event Action OnChange;
        bool IsVisible { get; set; }
        bool IsClickable { get; set; }
        EButtonType ButtonType { get; set; }
        string Title { get; set; }

        void Invoke();
    }

    public interface IStory : IButtonBase
    {

    }

    public abstract class ButtonBase : IButtonBase
    {
        public event Action OnChange;

        protected void NotifyStateChanged()
        {
            OnChange?.Invoke();
            _state.NotifyStateChanged();
        }

        protected readonly ApplicationState _state;
        public bool IsVisible { get; set; }
        public bool IsClickable { get; set; }
        public virtual EButtonType ButtonType { get; set; }
        public string Title { get; set; }
        public abstract void Invoke();

        protected ButtonBase(ApplicationState state)
        {
            _state = state;
        }
    }

    public class StoryButton : ButtonBase, IStory
    {
        private readonly IStoryService _storyService;
        private Timer _timer;

        public StoryButton(ApplicationState state, IStoryService storyService) : base (state)
        {
            _storyService = storyService;
            IsVisible = true;
            IsClickable = true;
            Title = "create fire";
        }

        public override void Invoke()
        {
            Title = "stoke fire";
            IsClickable = false;
            


            NotifyStateChanged();
            _timer?.Dispose();
            _timer = new Timer(Callback, null, 1_000, -1);
        }

        private void Callback(object state)
        {
            IsClickable = true;
            NotifyStateChanged();

            _storyService.Invoke();
        }
    }
}
