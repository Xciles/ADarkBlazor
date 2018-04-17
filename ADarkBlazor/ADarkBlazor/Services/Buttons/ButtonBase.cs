using System;
using ADarkBlazor.Services.Domain.Enums;

namespace ADarkBlazor.Services.Buttons
{
    public interface IButtonBase
    {
        event Action OnChange;
        bool IsVisible { get; set; }
        bool IsClickable { get; set; }
        EButtonType ButtonType { get; set; }

        bool Invoke();
    }

    public interface IStory : IButtonBase
    {

    }


    public abstract class ButtonBase : IButtonBase
    {
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        private readonly ApplicationState _state;
        public bool IsVisible { get; set; }
        public bool IsClickable { get; set; }
        public virtual EButtonType ButtonType { get; set; }
        public abstract bool Invoke();

        protected ButtonBase(ApplicationState state)
        {
            _state = state;
        }
    }

    public class StoryButton : ButtonBase, IStory
    {
        public override bool Invoke()
        {
            throw new NotImplementedException();
        }

        public StoryButton(ApplicationState state) : base(state)
        {
        }
    }
}
