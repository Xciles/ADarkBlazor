using System;
using System.Collections.Generic;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;

namespace ADarkBlazor.Services
{
    public class StoryService : IStoryService
    {
        public event Action OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();

        private readonly ApplicationState _state;
        public IList<OutputInfo> StoryOutputs { get; set; } = new List<OutputInfo>();
        private EStoryProgression _progression = EStoryProgression.Initial;


        public StoryService(ApplicationState state)
        {
            _state = state;

            AddOutput("the field is icy cold");
            AddOutput("there is no fire...");
        }

        private void AddOutput(string info)
        {
            StoryOutputs.Insert(0, new OutputInfo { Info = info });

            if (StoryOutputs.Count > 30) StoryOutputs.RemoveAt(30);

            NotifyStateChanged();
        }

        public void Invoke(EStoryEventType storyEventType = EStoryEventType.None)
        {
            switch (storyEventType)
            {
                case EStoryEventType.StoryEvent:
                {
                    HandleStory();
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        private void HandleStory()
        {
            AddOutput("there is no fire...");
            _progression++;
        }
    }
}