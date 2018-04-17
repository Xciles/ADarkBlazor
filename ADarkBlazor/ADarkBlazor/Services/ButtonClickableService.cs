using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services
{
    public class ButtonClickableService : IButtonClickableService
    {
        public bool ButtonEvent { get; set; }

        private IDictionary<EButtonType, Timer> _timers = new Dictionary<EButtonType, Timer>();

        public void HandleButton(EButtonType buttonType)
        {
            if (!_timers.ContainsKey(buttonType))
            {
            }
        }
    }
}
