using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.GameEngine.Animation
{
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorObservable : MonoBehaviour
    {
        public event StateDelegate OnStateEntered;

        public event StateDelegate OnStateExited; 

        public event Action<AnimationClip> OnAnimationStarted;

        public event Action<AnimationClip> OnAnimationEnded;
        
        public event Action OnEventReceived;

        public event Action<bool> OnBoolReceived;

        public event Action<int> OnIntReceived;

        public event Action<float> OnFloatReceived;

        public event Action<string> OnStringReceived;
        
        public event Action<Object> OnObjectReceived;

        private Animator animator;

        public void OnEnterState(AnimatorStateInfo state, int stateId, int layerIndex)
        {
            OnStateEntered?.Invoke(state, stateId, layerIndex);
        }
        
        public void OnExitState(AnimatorStateInfo state, int stateId, int layerIndex)
        {
            OnStateExited?.Invoke(state, stateId, layerIndex);
        }

        public void ReceiveStartAnimation(AnimationClip clip)
        {
            OnAnimationStarted?.Invoke(clip);
        }

        public void ReceiveEndAnimation(AnimationClip clip)
        {
            OnAnimationEnded?.Invoke(clip);
        }
        
        public void ReceiveString(string message) 
        {
            OnStringReceived?.Invoke(message);
        }

        public void ReceiveBool(bool message)
        {
            OnBoolReceived?.Invoke(message);
        }

        public void ReceiveInt(int message)
        {
            OnIntReceived?.Invoke(message);
        }

        public void ReceiveFloat(float message)
        {
            OnFloatReceived?.Invoke(message);
        }

        public void ReceiveReference(Object obj)
        {
            OnObjectReceived?.Invoke(obj);
        }
        
        public void ReceiveEvent()
        {
            OnEventReceived?.Invoke();
        }
    }
}