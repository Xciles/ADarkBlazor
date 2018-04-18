using System;
using System.Threading;
using ADarkBlazor.Services.Domain.Enums;

namespace ADarkBlazor.Services.Buttons
{
    public interface IButtonBase
    {
        event Action OnChange;
        bool IsVisible { get; set; }
        bool IsClickable { get; set; }
        EButtonType ButtonType { get; set; }

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
        public abstract void Invoke();

        protected ButtonBase(ApplicationState state)
        {
            _state = state;
        }
    }

    public class StoryButton : ButtonBase, IStory
    {
        private Timer _timer;

        public StoryButton(ApplicationState state) : base (state)
        {
            var var = "";
            string @string = "";
            IsVisible = true;
            IsClickable = true;
        }

        public override void Invoke()
        {
            IsClickable = false;
            NotifyStateChanged();
            _state.AddRoomInfo();
            _timer?.Dispose();
            _timer = new Timer(Callback, null, 1_000, -1);
        }

        private void Callback(object state)
        {
            IsClickable = true;
            NotifyStateChanged();
        }
    }
}
