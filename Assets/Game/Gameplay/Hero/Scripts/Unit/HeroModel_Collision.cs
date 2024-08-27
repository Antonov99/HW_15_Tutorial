using System;
using Elementary;
using Game.GameEngine.Mechanics;
using Declarative;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public sealed class HeroModel_Collision
    {
        [SerializeField]
        public ColliderDetection collisionSensor;

        [SerializeField]
        public TriggerSensor triggerSensor;

        [SerializeField]
        public Collider collider;

        private readonly BoolMechanics boolMechanics = new();

        [Construct]
        private void ConstructBoolMechanics(HeroModel_Core core)
        {
            boolMechanics.Construct(core.main.isEnable, isEnable =>
            {
                collider.enabled = isEnable;

                if (isEnable)
                    collisionSensor.Play();
                else
                    collisionSensor.Stop();
            });
        }
    }
}