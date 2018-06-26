using System;

namespace ADarkBlazor.Services.Buildings.Interfaces
{
    public interface IBuilding
    {

        event Action OnChange;
        bool IsVisible { get; set; }
        string Name { get; set; }
        int NumberOfBuildings { get; set; }
        bool IsUnlocked { get; }
        int BuildTime { get; }

        void Build();
        void Demolish();

        void Enable();
    }
}
