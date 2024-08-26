using Entities;
using UnityEngine;

namespace Lessons.Game.Events
{
    public readonly struct MoveEvent
    {
        public readonly IEntity Entity;
        public readonly Vector2Int Direction;

        public MoveEvent(IEntity entity, Vector2Int direction)
        {
            Entity = entity;
            Direction = direction;
        }
    }
}