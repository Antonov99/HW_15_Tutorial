using Entities;

namespace Lessons.Game.Events
{
    public readonly struct DealDamageEvent
    {
        public readonly IEntity Entity;
        public readonly int Strength;

        public DealDamageEvent(IEntity entity, int strength)
        {
            Entity = entity;
            Strength = strength;
        }
    }
}