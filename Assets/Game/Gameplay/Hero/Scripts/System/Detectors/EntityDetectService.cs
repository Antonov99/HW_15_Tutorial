using System.Collections.Generic;
using Entities;
using Game.GameEngine.Mechanics;
using GameSystem;
using Sirenix.OdinInspector;

namespace Game.Gameplay.Hero
{
    public sealed class EntityDetectService :
        IGameInitElement,
        IGameReadyElement,
        IGameFinishElement
    {
        [ReadOnly]
        [ShowInInspector]
        private readonly List<IEntity> detectedEntities = new();

        [ReadOnly]
        [ShowInInspector]
        private readonly List<EntityDetectListener> observers = new ();

        private HeroService heroService;

        private IComponent_ColliderSensor heroComponent;

        public List<IEntity> GetDetectedEntitesUnsafe()
        {
            return detectedEntities;
        }

        [GameInject]
        public void Construct(HeroService heroService)
        {
            this.heroService = heroService;
        }

        public void AddListener(EntityDetectListener listener)
        {
            observers.Add(listener);
        }

        public void RemoveListener(EntityDetectListener listener)
        {
            observers.Remove(listener);
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_ColliderSensor>();
        }

        void IGameReadyElement.ReadyGame()
        {
            heroComponent.OnCollisionsUpdated += UpdateEntities;
        }

        void IGameFinishElement.FinishGame()
        {
            heroComponent.OnCollisionsUpdated -= UpdateEntities;
        }

        private void UpdateEntities()
        {
            detectedEntities.Clear();
            heroComponent.GetCollidersUnsafe(out var buffer, out var size);

            for (var i = 0; i < size; i++)
            {
                var collider = buffer[i];
                if (collider.TryGetComponent(out IEntity entity))
                {
                    detectedEntities.Add(entity);
                }
            }

            for (int i = 0, count = observers.Count; i < count; i++)
            {
                var listener = observers[i];
                listener.OnEntitiesUpdated(detectedEntities);
            }
        }
    }
}