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
        string Title { get; set; }
        int Cooldown { get; set; }
        int RemainingCooldown { get; set; }

        void Invoke();
    }

    public interface IBuilderButtonBase
    {
        event Action OnChange;
        bool IsVisible { get; set; }
        bool IsClickable { get; set; }
        EButtonType ButtonType { get; set; }
        string Title { get; set; }
        int Cooldown { get; }
        int RemainingCooldown { get; set; }

        void Invoke();
    }
}