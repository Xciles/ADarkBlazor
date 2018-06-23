using System;
using System.Threading;
using ADarkBlazor.Services.Domain.Enums;

namespace ADarkBlazor.Services.Buttons
{
    public abstract class ButtonBase : IButtonBase
    {
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

        public virtual void Invoke()
        {
            if (IsClickable)
            {
                IsClickable = false;
                RemainingCooldown = Cooldown;

                InvokeImplementation();

                NotifyStateChanged();

                _timer?.Dispose();
                _timer = new Timer(ButtonCallback, null, 0, Interval);
            }
        }

        private void ButtonCallback(object state)
        {
            RemainingCooldown -= Interval;
            if (RemainingCooldown <= 0)
            {
                _timer?.Dispose();

                IsClickable = true;
                NotifyStateChanged();

                TimerFinished();
            }
        }

        public abstract void InvokeImplementation();
        public abstract void TimerFinished();

        protected ButtonBase(ApplicationState state)
        {
            State = state;
        }
    }
}
