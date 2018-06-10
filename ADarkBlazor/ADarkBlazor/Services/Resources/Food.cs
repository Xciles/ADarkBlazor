using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services.Resources
{
    public class Food : Resource, IFood
    {
        public Food(IResourceService resourceService) : base(resourceService)
        {
            Name = "Food";
            Amount = 100;
            ResourceType = EResourceType.Food;
        }
    }
}
