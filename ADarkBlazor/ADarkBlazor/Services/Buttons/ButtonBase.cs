using System;
using ADarkBlazor.Services.Domain.Enums;

namespace ADarkBlazor.Services.Buttons
{
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
}
