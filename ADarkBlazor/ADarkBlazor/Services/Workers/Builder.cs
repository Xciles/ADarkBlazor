using ADarkBlazor.Services.Resources.Interfaces;
using ADarkBlazor.Services.Workers.Interfaces;

namespace ADarkBlazor.Services.Workers
{
    public class Builder : Crafter, IBuilder
    {
        public Builder(IFood food, IHyperState hyperState) : base(food, hyperState)
        {
        }
    }
}