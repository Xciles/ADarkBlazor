namespace ADarkBlazor.Services.Buttons
{
    class DebugHyperButton : ButtonBase, IDebugHyper
    {

        public DebugHyperButton(ApplicationState state) : base(state)
        {
            Title = "Hyper";
            //Cooldown = 500;
            IsVisible = true;
            IsClickable = true;
        }

        public override void InvokeImplementation()
        {
            State.Hyper = !State.Hyper;
            Title = (State.Hyper) ? "unhyper" : "hyper";
        }

        public override void TimerFinished()
        {
        }
    }
}
