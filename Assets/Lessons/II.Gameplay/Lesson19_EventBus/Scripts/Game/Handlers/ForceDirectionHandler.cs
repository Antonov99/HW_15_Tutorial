using System;
using JetBrains.Annotations;
using Lessons.Entities.Common.Components;
using Lessons.Game.Events;
using Lessons.Level;
using UnityEngine;
using VContainer.Unity;

namespace Lessons.Game.Handlers
{
    [UsedImplicitly]
    public sealed class ForceDirectionHandler : IInitializable, IDisposable
    {
        private readonly LevelMap _levelMap;
        private readonly EventBus _eventBus;

        public ForceDirectionHandler(EventBus eventBus, LevelMap levelMap)
        {
            _eventBus = eventBus;
            _levelMap = levelMap;
        }

        void IInitializable.Initialize()
        {
            _eventBus.Subscribe<ForceDirectionEvent>(OnApplyForceDirection);
        }

        void IDisposable.Dispose()
        {
            _eventBus.Unsubscribe<ForceDirectionEvent>(OnApplyForceDirection);
        }

        private void OnApplyForceDirection(ForceDirectionEvent evt)
        {
            var coordinates = evt.Entity.Get<CoordinatesComponent>();
            Vector2Int destination = coordinates.Value + evt.Direction;

            if (_levelMap.Entities.HasEntity(destination))
            {
                _eventBus.RaiseEvent(new DealDamageEvent(evt.Entity, 1));
                _eventBus.RaiseEvent(new 
                    DealDamageEvent(_levelMap.Entities.GetEntity(destination), 1));
            }
            else
            {
                _eventBus.RaiseEvent(new MoveEvent(evt.Entity, evt.Direction));
            }
        }
    }
}