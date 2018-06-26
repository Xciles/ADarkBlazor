using System;
using System.Collections.Generic;

namespace ADarkBlazor.Services.Domain
{
    public class ResourceState
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public bool IsVisible { get; set; }
    }

    public class SaveState
    { 
        public DateTime Time { get; set; }
        public IList<ResourceState> Resources { get; set; } = new List<ResourceState>(); // Saving as dict does not work atm... Does not get serialized properly
    }
}
