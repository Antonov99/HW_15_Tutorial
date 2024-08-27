using System;
using Elementary;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.GameEngine.Animation
{
    [AddComponentMenu("GameEngine/Animation/Animator Machine")]
    public class UAnimatorMachine : MonoBehaviour
    {
        private static readonly int STATE_PARAMETER = Animator.StringToHash("State");

        public event StateDelegate OnStateEntered;

        public event StateDelegate OnStateExited;

        public event Action OnEventReceived
        {
            add { eventDispatcher.OnEventReceived += value; }
            remove { eventDispatcher.OnEventReceived -= value; }
        }

        public event Action<bool> OnBoolReceived
        {
            add { eventDispatcher.OnBoolReceived += value; }
            remove { eventDispatcher.OnBoolReceived -= value; }
        }

        public event Action<int> OnIntReceived
        {
            add { eventDispatcher.OnIntReceived += value; }
            remove { eventDispatcher.OnIntReceived -= value; }
        }

        public event Action<float> OnFloatReceived
        {
            add { eventDispatcher.OnFloatReceived += value; }
            remove { eventDispatcher.OnFloatReceived -= value; }
        }

        public event Action<string> OnStringReceived
        {
            add { eventDispatcher.OnStringReceived += value; }
            remove { eventDispatcher.OnStringReceived -= value; }
        }

        public event Action<Object> OnObjectReceived
        {
            add { eventDispatcher.OnObjectReceived += value; }
            remove { eventDispatcher.OnObjectReceived -= value; }
        }

        public event Action<AnimationClip> OnAnimationStarted
        {
            add { eventDispatcher.OnAnimationStarted += value; }
            remove { eventDispatcher.OnAnimationStarted -= value; }
        }

        public event Action<AnimationClip> OnAnimationFinished
        {
            add { eventDispatcher.OnAnimationEnded += value; }
            remove { eventDispatcher.OnAnimationEnded -= value; }
        }

        [PropertySpace]
        [PropertyOrder(-10)]
        [LabelText("Apply Root Motion")]
        [ReadOnly]
        [ShowInInspector]
        public bool IsRootMotion
        {
            get { return animator != null && animator.applyRootMotion; }
        }

        public float BaseSpeed
        {
            get { return baseSpeed; }
        }

        public int CurrentState
        {
            get { return stateId; }
        }

        [ReadOnly]
        [ShowInInspector]
        private int stateId;

        [ReadOnly]
        [ShowInInspector]
        private float baseSpeed;
        
        [Space]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private AnimatorObservable eventDispatcher;
        
        private readonly AnimatorMultiplierComposite multiplier = new();

        [SerializeField]
        private StateHolder[] states = Array.Empty<StateHolder>();

        protected virtual void Awake()
        {
            stateId = animator.GetInteger(STATE_PARAMETER);
            baseSpeed = animator.speed;
        }

        protected virtual void OnEnable()
        {
            eventDispatcher.OnStateEntered += OnEnterState;
            eventDispatcher.OnStateExited += OnExitState;
        }

        protected virtual  void OnDisable()
        {
            eventDispatcher.OnStateEntered -= OnEnterState;
            eventDispatcher.OnStateExited -= OnExitState;
        }

        public void PlayAnimation(string animationName, string layerName, float normalizedTime = 0)
        {
            var id = Animator.StringToHash(animationName);
            PlayAnimation(id, layerName, normalizedTime);
        }

        public void PlayAnimation(int hash, string layerName, float normalizedTime = 0)
        {
            var index = animator.GetLayerIndex(layerName);
            PlayAnimation(hash, index, normalizedTime);
        }

        public void SetLayerWeight(int layer, float weight)
        {
            animator.SetLayerWeight(layer, weight);
        }

        public void PlayAnimation(int hash, int layer, float normalizedTime = 0)
        {
            animator.Play(hash, layer, normalizedTime);
        }

        public void ChangeState(int stateId)
        {
            if (this.stateId == stateId)
            {
                return;
            }

            this.stateId = stateId;
            animator.SetInteger(STATE_PARAMETER, this.stateId);
        }

        public void AddSpeedMultiplier(IAnimatorMultiplier multiplier)
        {
            this.multiplier.Add(multiplier);
            UpdateAnimatorSpeed();
        }

        public void RemoveSpeedMultiplier(IAnimatorMultiplier multiplier)
        {
            this.multiplier.Remove(multiplier);
            UpdateAnimatorSpeed();
        }

        public void SetBaseSpeed(float speed)
        {
            if (Mathf.Approximately(speed, baseSpeed))
            {
                return;
            }

            baseSpeed = speed;
            UpdateAnimatorSpeed();
        }

        public void ApplyRootMotion()
        {
            animator.applyRootMotion = true;
        }

        public void ResetRootMotion(bool resetPosition = true, bool resetRotation = true)
        {
            animator.applyRootMotion = false;
            if (resetPosition)
            {
                animator.transform.localPosition = Vector3.zero;
            }

            if (resetRotation)
            {
                animator.transform.localRotation = Quaternion.identity;
            }
        }

        private void OnEnterState(AnimatorStateInfo state, int stateId, int layerindex)
        {
            if (FindState(stateId, out var fsmState))
            {
                fsmState.Enter();
            }

            OnStateEntered?.Invoke(state, stateId, layerindex);
        }

        private void OnExitState(AnimatorStateInfo state, int stateId, int layerindex)
        {
            if (FindState(stateId, out var fsmState))
            {
                fsmState.Exit();
            }

            OnStateExited?.Invoke(state, stateId, layerindex);
        }

        private void UpdateAnimatorSpeed()
        {
            animator.speed = baseSpeed * multiplier.GetValue();
        }

        private bool FindState(int id, out MonoState state)
        {
            for (int i = 0, count = states.Length; i < count; i++)
            {
                StateHolder holder = states[i];
                if (holder.id == id)
                {
                    state = holder.state;
                    return true;
                }
            }

            state = default;
            return false;
        }

        [Serializable]
        private struct StateHolder
        {
            [SerializeField]
            public int id;

            [SerializeField]
            public MonoState state;
        }
    }
}