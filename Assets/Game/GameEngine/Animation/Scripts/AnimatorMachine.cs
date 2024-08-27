using System;
using System.Collections.Generic;
using Elementary;
using Declarative;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.GameEngine.Animation
{
    [Serializable]
    public class AnimatorMachine :
        IAwakeListener,
        IEnableListener,
        IUpdateListener,
        IDisableListener
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
        [LabelText("Apply Root Motion")]
        [ReadOnly]
        [ShowInInspector]
        public bool IsRootMotion
        {
            get { return animator != null && animator.applyRootMotion; }
        }

        [ReadOnly]
        [ShowInInspector]
        public float BaseSpeed
        {
            get { return baseSpeed; }
        }

        [ReadOnly]
        [ShowInInspector]
        public int CurrentState
        {
            get { return stateId; }
        }

        private int stateId;

        private float baseSpeed;

        [ShowInInspector, ReadOnly, PropertySpace]
        private readonly AnimatorMultiplierComposite multiplier = new();

        [ShowInInspector, ReadOnly]
        public List<StateEntry> states = new();

        [ShowInInspector, ReadOnly]
        public List<StateTransition> orderedTransitions = new();

        private Animator animator;

        private AnimatorObservable eventDispatcher;

        public void Construct(Animator animator, AnimatorObservable eventDispatcher)
        {
            this.animator = animator;
            this.eventDispatcher = eventDispatcher;
        }

        public bool AddState(int id, IState state)
        {
            if (FindState(id, out _))
            {
                return false;
            }

            states.Add(new StateEntry
            {
                id = id,
                state = state
            });

            return true;
        }

        public bool RemoveState(int id)
        {
            for (int i = 0, count = states.Count; i < count; i++)
            {
                var holder = states[i];
                if (holder.id.Equals(id))
                {
                    states.Remove(holder);
                    return true;
                }
            }

            return false;
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

        public void SwitchState(int stateId)
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

        public virtual void Awake()
        {
            stateId = animator.GetInteger(STATE_PARAMETER);
            baseSpeed = animator.speed;
        }

        public virtual void OnEnable()
        {
            eventDispatcher.OnStateEntered += OnEnterState;
            eventDispatcher.OnStateExited += OnExitState;
        }

        public virtual void Update(float deltaTime)
        {
            UpdateTransitions();
        }

        public virtual void OnDisable()
        {
            eventDispatcher.OnStateEntered -= OnEnterState;
            eventDispatcher.OnStateExited -= OnExitState;
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

        private bool FindState(int id, out IState state)
        {
            for (int i = 0, count = states.Count; i < count; i++)
            {
                var holder = states[i];
                if (holder.id.Equals(id))
                {
                    state = holder.state;
                    return true;
                }
            }

            state = default;
            return false;
        }
        
        
        private void UpdateTransitions()
        {
            for (int i = 0, count = orderedTransitions.Count; i < count; i++)
            {
                var transition = orderedTransitions[i];
                if (transition.condition.IsTrue())
                {
                    SwitchState(transition.id);
                    return;
                }
            }
        }

        public struct StateEntry
        {
            public int id;
            
            public IState state;

            public StateEntry(int id, IState state)
            {
                this.id = id;
                this.state = state;
            }
        }

        public struct StateTransition
        {
            public int id;
            
            public ICondition condition;

            public StateTransition(int id, ICondition condition)
            {
                this.id = id;
                this.condition = condition;
            }

            public StateTransition(int id, Func<bool> condition)
            {
                this.id = id;
                this.condition = new ConditionDelegate(condition);  
            }
        }
    }
}