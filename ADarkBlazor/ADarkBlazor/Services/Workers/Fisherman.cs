using ADarkBlazor.Services.Resources.Interfaces;
using ADarkBlazor.Services.Workers.Interfaces;

namespace ADarkBlazor.Services.Workers
{
    public class Fisherman : Gatherer, IFisherman
    {
        public Fisherman(IFood food, IHyperState hyperState) : base(hyperState, food)
        {
            Name = "Fisherman";
            AmountPer10Seconds = 10;
        }

        protected override void CallbackImplementation()
        {
        }
    }
}