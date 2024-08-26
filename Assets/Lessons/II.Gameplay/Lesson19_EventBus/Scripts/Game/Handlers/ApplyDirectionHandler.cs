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
    public class ApplyDirectionHandler : IInitializable, IDisposable
    {
        private readonly LevelMap _levelMap;
        private readonly EventBus _eventBus;

        public ApplyDirectionHandler(LevelMap levelMap, EventBus eventBus)
        {
            _levelMap = levelMap;
            _eventBus = eventBus;
        }

        void IInitializable.Initialize()
        {
            _eventBus.Subscribe<ApplyDirectionEvent>(OnApplyDirection);
        }

        void IDisposable.Dispose()
        {
            _eventBus.Unsubscribe<ApplyDirectionEvent>(OnApplyDirection);
        }

        private void OnApplyDirection(ApplyDirectionEvent evt)
        {
            CoordinatesComponent coordinates = evt.Entity.Get<CoordinatesComponent>();
            Vector2Int targetCoordinates = coordinates.Value + evt.Direction;

            if (_levelMap.Entities.HasEntity(targetCoordinates))
            {
                _eventBus.RaiseEvent(new AttackEvent(evt.Entity, _levelMap.Entities.GetEntity(targetCoordinates)));
                return;
            }

            if (_levelMap.Tiles.IsWalkable(targetCoordinates))
            {
                _eventBus.RaiseEvent(new MoveEvent(evt.Entity, evt.Direction));
            }
        }
    }
}