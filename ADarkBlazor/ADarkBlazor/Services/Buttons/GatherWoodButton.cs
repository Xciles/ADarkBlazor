using System;
using System.Threading;
using ADarkBlazor.Exceptions;
using ADarkBlazor.Services.Buttons.Interfaces;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Resources;
using ADarkBlazor.Services.Resources.Interfaces;

namespace ADarkBlazor.Services.Buttons
{
    public class GatherWoodButton : ButtonBase, IGatherWood
    {
        private readonly IStoryService _storyService;
        private readonly IWood _wood;

        public GatherWoodButton(ApplicationState state, IStoryService storyService, IWood wood) : base(state)
        {
            _storyService = storyService;
            _wood = wood;
            IsVisible = true;
            IsClickable = true;
            Title = "gather wood";
            Cooldown = 20_000;
        }

        public override void InvokeImplementation()
        {
            _storyService.Invoke(EStoryEventType.GatherWood);
            _wood.Add(20);
        }

        public override void TimerFinished()
        {
        }
    }
}