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
            _state.Hyper = !_state.Hyper;
            Title = (_state.Hyper) ? "unhyper" : "hyper";
        }

        public override void TimerFinished()
        {
        }
    }
}
