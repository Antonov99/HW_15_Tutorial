using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Animation
{
    [AddComponentMenu("GameEngine/Animation/Animator State «Change State»")]
    public sealed class UAnimatorState_ChangeState : MonoState
    {
        [SerializeField]
        private UAnimatorMachine system;

        [SerializeField]
        private IntAdapter enterId;

        [Space]
        [SerializeField]
        private bool hasExitAnimation;

        [ShowIf("hasExitAnimation")]
        [OptionalField]
        [SerializeField]
        private IntAdapter exitId;
        
        public override void Enter()
        {
            system.ChangeState(enterId.Current);
        }

        public override void Exit()
        {
            if (hasExitAnimation)
            {
                system.ChangeState(exitId.Current);
            }
        }
    }
}