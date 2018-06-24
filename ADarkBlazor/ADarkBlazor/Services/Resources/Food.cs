using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services.Resources
{
    public class Food : Resource, IFood
    {
        public Food(IResourceService resourceService) : base(resourceService)
        {
            Name = "Food";
            Reset();
            ResourceType = EResourceType.Food;
        }

        public override void Reset()
        {
            IsVisible = false;
            Amount = 100;

            NotifyStateChanged();
        }
    }
}
