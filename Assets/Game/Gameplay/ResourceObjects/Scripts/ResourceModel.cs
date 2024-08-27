using System;
using Elementary;
using Entities;
using Game.GameEngine;
using Game.GameEngine.Mechanics;
using Declarative;
using UnityEngine;

namespace Game.Gameplay.ResourceObjects
{
    public sealed class ResourceModel : DeclarativeModel
    {
        [Section]
        [SerializeField]
        private ScriptableResource config;

        [Section]
        [SerializeField, Space]
        private Core core;

        [Section]
        [SerializeField]
        private Components components;

        [Section]
        [SerializeReference]
        private IVisual visual;

        [Serializable]
        public sealed class Core
        {
            [SerializeField]
            public Transform rootTransform;

            [SerializeField]
            public GameObject collisionLayer;

            [Space]
            [SerializeField]
            public BoolVariable isActive = new();

            [SerializeField]
            public Emitter takeHitEvent = new();

            [SerializeField]
            public Emitter destroyEvent = new();
            
            [SerializeField]
            public Emitter respawnEvent = new();
            
            private readonly BoolMechanics activeMechanics = new();

            private readonly RespawnMechanics respawnMechanics = new();

            [Construct]
            private void ConstructActiveMechanics()
            {
                activeMechanics.Construct(isActive, collisionLayer.SetActive);
            }

            [Construct]
            private void ConstructDestroy()
            {
                destroyEvent.AddListener(() => isActive.Current = false);
            }

            [Construct]
            private void ConstructRespawn(MonoBehaviour context, ScriptableResource config)
            {
                respawnMechanics.ConstructDuration(config.respawnTime);
                respawnMechanics.ConstructDestroyEvent(destroyEvent);
                respawnMechanics.ConstructRespawnEvent(respawnEvent);

                respawnEvent.AddListener(() => isActive.Current = true);
            }
        }

        [Serializable]
        public sealed class Components
        {
            [SerializeField]
            private MonoEntityStd entity;

            [Construct]
            private void Construct(ScriptableResource config, Core core)
            {
                entity.AddRange(
                    new Component_Transform(core.rootTransform),
                    new Component_ObjectType(config.objectType),
                    new Component_ResourceInfo(config),
                    new Component_Hit(core.takeHitEvent),
                    new Component_CanDestoy_BoolVariable(core.isActive),
                    new Component_Destroy_Emitter(core.destroyEvent)
                );
            }
        }

        public interface IVisual
        {
        }

        [Serializable]
        public class BaseVisual : IVisual
        {
            [SerializeField]
            private GameObject visualObject;

            [Construct]
            private void Construct(Core core)
            {
                core.isActive.AddListener(visualObject.SetActive);
            }
        }

        [Serializable]
        public class TreeVisual : BaseVisual
        {
            [SerializeField]
            private Animator animator;

            [Construct]
            private void Construct(Core core)
            {
                core.takeHitEvent.AddListener(() => animator.Play("Chop", -1, 0));
            }
        }
    }
}