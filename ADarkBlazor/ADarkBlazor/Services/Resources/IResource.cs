using System;
using ADarkBlazor.Services.Domain.Enums;

namespace ADarkBlazor.Services.Resources
{
    public interface IResource : IHasSaveState
    {
        event Action OnChange;
        bool IsVisible { get; set; }
        EResourceType ResourceType { get; set; }
        string Name { get; set; }
        double Amount { get; set; }

        void Add(double amount);
        void Subtract(double amount);

        void Enable();
    }
}