using System;
using JetBrains.Annotations;
using Lessons.Entities.Common.Components;
using Lessons.Game.Events;
using Lessons.Game.Events.Effects;
using UnityEngine;
using VContainer.Unity;

namespace Lessons.Game.Handlers.Effects
{
    [UsedImplicitly]
    public sealed class PushEffectHandler : IInitializable, IDisposable
    {
        private readonly EventBus _eventBus;

        public PushEffectHandler(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        void IInitializable.Initialize()
        {
            _eventBus.Subscribe<PushEffectEvent>(OnPush);
        }

        void IDisposable.Dispose()
        {
            _eventBus.Unsubscribe<PushEffectEvent>(OnPush);
        }

        private void OnPush(PushEffectEvent evt)
        {
            var selfCoordinates = evt.Source.Get<CoordinatesComponent>();
            var targetCoordinates = evt.Target.Get<CoordinatesComponent>();
                
            Vector2Int direction = targetCoordinates.Value - selfCoordinates.Value;
                
            _eventBus.RaiseEvent(new ForceDirectionEvent(evt.Target, direction));
        }
    }
}