using ADarkBlazor.Services.Interfaces;

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
