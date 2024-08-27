using System.Collections.Generic;
using Elementary;
using UnityEngine;
using UnityEngine.Events;

namespace Game.GameEngine.Animation
{
    [AddComponentMenu("GameEngine/Animation/Animator State Event «Invoke Unity Event»")]
    public sealed class UAnimatorStateEvent_InvokeUnityEvent : MonoState
    {
        [Space]
        [SerializeField]
        private UAnimatorMachine animationSystem;
        
        [SerializeField]
        private UnityEvent unityEvent;

        [Space]
        [SerializeField]
        private List<string> animationEvents = new()
        {
            "harvest"
        };

        public override void Enter()
        {
            animationSystem.OnStringReceived += OnAnimationEvent;
        }

        public override void Exit()
        {
            animationSystem.OnStringReceived -= OnAnimationEvent;
        }

        private void OnAnimationEvent(string message)
        {
            if (animationEvents.Contains(message))
            {
                unityEvent.Invoke();
            }
        }
    }
}