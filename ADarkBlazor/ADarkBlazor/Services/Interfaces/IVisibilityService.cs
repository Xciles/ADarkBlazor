using System;
using System.Collections.Generic;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Domain.Enums;

namespace ADarkBlazor.Services.Interfaces
{
    public interface IVisibilityService
    {
        event Action OnChange;
        IList<MenuItem> Menu { get; set; }
        void Unlock(EMenuType menuType);
    }
}