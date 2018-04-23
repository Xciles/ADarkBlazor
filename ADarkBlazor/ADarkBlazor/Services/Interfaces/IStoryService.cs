using System;
using System.Collections.Generic;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Domain.Enums;

namespace ADarkBlazor.Services.Interfaces
{
    public interface IStoryService
    {
        event Action OnChange;
        IList<OutputInfo> StoryOutputs { get; set; }
        void Invoke(EStoryEventType storyEventType = EStoryEventType.None);
    }
}
