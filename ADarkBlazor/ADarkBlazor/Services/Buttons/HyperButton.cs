namespace ADarkBlazor.Services.Buttons
{
    public class HyperButton : ButtonBase, IHyper
    {

        public HyperButton(ApplicationState state) : base(state)
        {
            Title = "Hyper";
            IsVisible = true;
            IsClickable = true;
        }

        public override void InvokeImplementation()
        {
            State.HyperState.Enabled = !State.HyperState.Enabled;
            Title = (State.HyperState.Enabled) ? "unhyper" : "hyper";
        }

        public override void TimerFinished()
        {
        }
    }
}
