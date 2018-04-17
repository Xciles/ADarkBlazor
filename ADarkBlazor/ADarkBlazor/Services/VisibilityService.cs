﻿using System;
using System.Collections.Generic;
using System.Linq;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services
{
    public class VisibilityService : IVisibilityService
    {
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public IList<MenuItem> Menu { get; set; } = new List<MenuItem>();
        private IList<MenuItem> _availableMenuItems = new List<MenuItem>
        {
            new MenuItem{ Type = EMenuType.Test, Description = "Test Item", HRef = "/room", Icon = "glyphicon-education"}
        };

        public void Unlock(EMenuType menuType)
        {
            if (!Menu.Any(x => x.Type == menuType))
            {
                Menu.Add(_availableMenuItems.First(x => x.Type == menuType));
            }

            NotifyStateChanged();
        }
    }
}