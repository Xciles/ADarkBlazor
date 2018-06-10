using System;
using System.Collections.Generic;
using System.Threading;
using ADarkBlazor.Services.Domain;
using ADarkBlazor.Services.Domain.Enums;
using ADarkBlazor.Services.Interfaces;
using ADarkBlazor.Services.Workers;

namespace ADarkBlazor.Services
{
    public class StoryService : IStoryService
    {
        public event Action OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();

        private readonly ApplicationState _state;
        private readonly IResourceService _resourceService;
        private readonly IVisibilityService _visibilityService;
        private readonly IWorkerService _workerService;
        public IList<OutputInfo> StoryOutputs { get; set; } = new List<OutputInfo>();
        private EStoryProgression _progression = 0;


        public StoryService(ApplicationState state, IResourceService resourceService, IVisibilityService visibilityService, IWorkerService workerService)
        {
            _state = state;
            _resourceService = resourceService;
            _visibilityService = visibilityService;
            _workerService = workerService;

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
                case EStoryEventType.NoMoreWoodEvent:
                    {
                        AddOutput(@"there is no more wood, you have to find more...");
                        break;
                    }
                case EStoryEventType.GatherWood:
                    {
                        AddOutput(@"you slap a tree, you find a lot of wood");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void Invoke(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                AddOutput(message);
            }
        }

        private void HandleStory()
        {
            // Big bad switch
            // Replace with something else at a later stage...
            switch (_progression)
            {
                case EStoryProgression.LookAround:
                    {
                        AddOutput(@"you look around");
                        break;
                    }
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
                        Timer timer = new Timer((_) =>
                        {
                            AddOutput(@"a strange shivering creature joins you next to the fire");
                            AddOutput(@"you look past the creature and see a forest...");
                            _visibilityService.Unlock(EMenuType.Woods);
                            _resourceService.EnableResource(EResourceType.Food);
                        }, null, 7_500, -1);
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
                        Timer timer = new Timer((_) =>
                        {
                            AddOutput(@"the strange creature shifts into a humanoid form");
                            AddOutput(@"the strange creature says it can build stuff");
                            _workerService.EnableWorker(typeof(Builder));
                            _workerService.AddPersonToWorker(typeof(IdleWorker));
                            _workerService.AddPersonToWorker(typeof(Builder));
                            //_workerService.
                        }, null, 7_500, -1);
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