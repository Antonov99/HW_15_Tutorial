using System.Collections.Generic;
using Entities;
using Sirenix.OdinInspector;

namespace Game.Gameplay.Hero
{
    public abstract class EntityDetectListener
    {
        [ReadOnly]
        [ShowInInspector]
        private readonly List<IEntity> detectedEntites = new();
        
        protected abstract bool MatchesEntity(IEntity entity);
        
        protected abstract void OnEntitesChanged(List<IEntity> entities);

        public void OnEntitiesUpdated(List<IEntity> entities)
        {
            detectedEntites.Clear();
            for (int i = 0, count = entities.Count; i < count; i++)
            {
                var entity = entities[i];
                if (MatchesEntity(entity))
                {
                    detectedEntites.Add(entity);
                }
            }
            
            OnEntitesChanged(detectedEntites);
        }
    }
}