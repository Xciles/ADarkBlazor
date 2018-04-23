using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services
{
    public class StoryService : IStoryService
    {
        private readonly ApplicationState _state;

        public StoryService(ApplicationState state)
        {
            _state = state;
        }

        public void Invoke()
        {

        }
    }
}