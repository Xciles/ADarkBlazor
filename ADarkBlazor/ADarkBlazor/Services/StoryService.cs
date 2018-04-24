using System;
using System.Collections.Generic;
using System.Threading;
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
        private readonly IResourceService _resourceService;
        public IList<OutputInfo> StoryOutputs { get; set; } = new List<OutputInfo>();
        private EStoryProgression _progression = EStoryProgression.FindWood;


        public StoryService(ApplicationState state, IResourceService resourceService)
        {
            _state = state;
            _resourceService = resourceService;

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
            // Big bad switch
            // Replace with something else at a later stage...
            switch (_progression)
            {
                case EStoryProgression.FindWood:
                    {
                        AddOutput(@"you search the field for branches. you found something you can burn");
                        _resourceService.AddToResource(EResourceType.Wood, 10);
                        _resourceService.EnableResource(EResourceType.Wood);
                        break;
                    }
                case EStoryProgression.FireInitial:
                    {
                        AddOutput(@"you start to light the fire");
                        AddOutput(@"the fire starts to burn");
                        _resourceService.SubtractFromResource(EResourceType.Wood, 5);
                        break;
                    }
                case EStoryProgression.FireLit:
                    {
                        AddOutput(@"the area is still cold");
                        break;
                    }
                case EStoryProgression.FireCold:
                    {
                        AddOutput(@"the fire keeps burning");
                        Timer time = new Timer((_) => AddOutput(@"a strange shivering creature joins you next to the fire"), null, 5_000, -1);
                        break;
                    }
                case EStoryProgression.FireWarm:
                    {
                        AddOutput(@"the area is getting warmer");
                        break;
                    }
                case EStoryProgression.FireWarmer:
                    {
                        AddOutput(@"the area is warm");
                        break;
                    }
                case EStoryProgression.FireHot:
                    {
                        AddOutput(@"the area is hot");
                        Timer timer = new Timer((_) => AddOutput(@"the strange creature shifts into a humanoid form"), null, 5_000, -1);
                        break;
                    }
                case EStoryProgression.Initial:
                default:
                    {
                        AddOutput(@"the area is hot");

                        break;
                    }
            }
            _progression++;
        }
    }
}