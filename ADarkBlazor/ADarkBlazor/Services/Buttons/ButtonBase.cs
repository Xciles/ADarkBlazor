using System;
using System.Threading;
using ADarkBlazor.Services.Domain.Enums;

namespace ADarkBlazor.Services.Buttons
{
    public abstract class ButtonBase : IButtonBase
    {
        public event Action OnChange;
        private Timer _timer;
        private const int _interval = 100;

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
        public int Cooldown { get; set; }
        public int RemainingCooldown { get; set; }

        public int CalculatedStartFrom
        {
            get { return 0; }
        }

        public void Invoke()
        {
            if (IsClickable)
            {
                IsClickable = false;
                RemainingCooldown = Cooldown;
                
                InvokeImplementation();
                
                NotifyStateChanged();

                _timer?.Dispose();
                _timer = new Timer(Callback, null, 0, _interval);
            }
        }

        private void Callback(object state)
        {
            RemainingCooldown -= _interval;
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
            _state = state;
        }
    }
}
