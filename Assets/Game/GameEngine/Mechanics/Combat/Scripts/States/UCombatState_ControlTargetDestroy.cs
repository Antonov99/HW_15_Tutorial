using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Combat/Combat State «Control Target Destroy»")]
    public sealed class UCombatState_ControlTargetDestroy : MonoState
    {
        [Space, SerializeField]
        public UCombatOperator combatOperator;

        [SerializeField]
        public Object attacker;

        private IComponent_OnDestroyed<DestroyArgs> targetComponent;

        public override void Enter()
        {
            targetComponent = combatOperator
                .Current
                .targetEntity
                .Get<IComponent_OnDestroyed<DestroyArgs>>();
            targetComponent.OnDestroyed += OnTargetDestroyed;
        }

        public override void Exit()
        {
            targetComponent.OnDestroyed -= OnTargetDestroyed;
        }

        private void OnTargetDestroyed(DestroyArgs destroyArgs)
        {
            if (IsDestroyedByMe(destroyArgs))
            {
                combatOperator.Current.targetDestroyed = true;
            }

            combatOperator.Stop();
        }

        private bool IsDestroyedByMe(DestroyArgs destroyArgs)
        {
            return destroyArgs.reason == DestroyReason.ATTACKER &&
                   ReferenceEquals(destroyArgs.source, attacker);
        }
    }
}