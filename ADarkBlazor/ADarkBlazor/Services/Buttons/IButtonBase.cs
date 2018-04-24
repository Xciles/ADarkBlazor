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

        void Invoke();
    }
}