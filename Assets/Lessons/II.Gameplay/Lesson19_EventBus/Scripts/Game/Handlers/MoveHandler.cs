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
    public class MoveHandler : IInitializable, IDisposable
    {
        private readonly LevelMap _levelMap;
        private readonly EventBus _eventBus;

        public MoveHandler(LevelMap levelMap, EventBus eventBus)
        {
            _levelMap = levelMap;
            _eventBus = eventBus;
        }
        
        void IInitializable.Initialize()
        {
            _eventBus.Subscribe<MoveEvent>(OnMove);
        }

        void IDisposable.Dispose()
        {
            _eventBus.Unsubscribe<MoveEvent>(OnMove);
        }

        private void OnMove(MoveEvent evt)
        {
            CoordinatesComponent coordinates = evt.Entity.Get<CoordinatesComponent>();
            Vector2Int targetCoordinates = coordinates.Value + evt.Direction;
            
            _levelMap.Entities.RemoveEntity(coordinates.Value);
            _levelMap.Entities.SetEntity(targetCoordinates, evt.Entity);
            coordinates.Value = targetCoordinates;

            PositionComponent position = evt.Entity.Get<PositionComponent>();
            position.Value = _levelMap.Tiles.CoordinatesToPosition(targetCoordinates);

            if (!_levelMap.Tiles.IsWalkable(targetCoordinates))
            {
                _eventBus.RaiseEvent(new DestroyEvent(evt.Entity));
            }
        }
    }
}