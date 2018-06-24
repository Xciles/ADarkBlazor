using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources.Interfaces;

namespace ADarkBlazor.Services.Resources
{
    public class Wood : Resource, IWood
    {
        public Wood(IResourceService resourceService) : base(resourceService)
        {
            Name = "Wood";
        }
    }
}
