using System;
using JetBrains.Annotations;
using Lessons.Entities.Common.Components;
using Lessons.Game.Events;
using Lessons.Level;
using VContainer.Unity;

namespace Lessons.Game.Handlers
{
    [UsedImplicitly]
    public class DestroyHandler : IInitializable, IDisposable
    {
        private readonly LevelMap _levelMap;
        private readonly EventBus _eventBus;

        public DestroyHandler(LevelMap levelMap, EventBus eventBus)
        {
            _levelMap = levelMap;
            _eventBus = eventBus;
        }
        
        void IInitializable.Initialize()
        {
            _eventBus.Subscribe<DestroyEvent>(OnDestroy);
        }

        void IDisposable.Dispose()
        {
            _eventBus.Unsubscribe<DestroyEvent>(OnDestroy);
        }

        private void OnDestroy(DestroyEvent evt)
        {
            if (evt.Entity.TryGet(out DeathComponent deathComponent))
            {
                deathComponent.Die();
            }

            CoordinatesComponent coordinates = evt.Entity.Get<CoordinatesComponent>();
            _levelMap.Entities.RemoveEntity(coordinates.Value);

            if (evt.Entity.TryGet(out DestroyComponent destroyComponent))
            {
                destroyComponent.Destroy();
            }
        }
    }
}