using ADarkBlazor.Services.Resources.Interfaces;
using ADarkBlazor.Services.Workers.Interfaces;

namespace ADarkBlazor.Services.Workers
{
    public class WoodGatherer : Gatherer, IWoodGatherer
    {
        public WoodGatherer(IWood wood, IHyperState hyperState) : base(hyperState, wood)
        {
            Name = "Wood Gatherer";
        }

        protected override void CallbackImplementation()
        {
        }
    }
}