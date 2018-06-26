namespace ADarkBlazor.Services.Workers.Interfaces
{
    public class IdleWorker : Worker, IIdleWorker
    {
        public IdleWorker()
        {
            Name = "Idle Worker";
            //NumberOfWorkers = 4;
        }
    }
}