using Entities;
using Game.GameEngine;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.Gameplay.Dummies
{
    [RequireComponent(typeof(DummyModel))]
    public sealed class DummyEntity : MonoEntityBase
    {
        [SerializeField]
        private Transform rootTransform;

        private void Awake()
        {
            var model = GetComponent<DummyModel>();
            var config = model.config;
            var core = model.core;
            
            AddRange(
                new Component_Transform(rootTransform),
                new Component_ObjectType(config.objectType),
                new Component_GetName(config.dummyName),
                new Component_HitPoints(core.hitPointsEngine),
                new Component_IsAlive_HitPoints(core.hitPointsEngine),
                new Component_IsDestroyed_HitPoints(core.hitPointsEngine),
                new Component_TakeDamage(core.takeDamageEngine),
                new Component_Destroy_Emitter<DestroyArgs>(core.destroyEmitter)
            );
        }
    }
}