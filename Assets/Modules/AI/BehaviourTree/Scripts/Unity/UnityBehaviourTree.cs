using System;
using UnityEngine;

namespace AI.BTree
{
    [AddComponentMenu(Extensions.MENU_PATH + "Behaviour Tree")]
    public sealed class UnityBehaviourTree : UnityBehaviourNode,
        IBehaviourTree,
        IBehaviourCallback
    {
        public event Action OnStarted;
        public event Action<bool> OnFinished;
        public event Action OnAborted;

        public bool IsEnable
        {
            get { return enabled; }
            set { enabled = value; }
        }

        [SerializeField]
        private bool autoRun = true;

        [SerializeField]
        private bool loop = true;

        [SerializeField]
        private UpdateMode updateMode;

        [SerializeField]
        private UnityBehaviourNode root;

        private void Start()
        {
            if (autoRun)
            {
                Run();
            }
        }

        private void Update()
        {
            if (loop && updateMode == UpdateMode.UPDATE)
            {
                Run();
            }
        }

        private void FixedUpdate()
        {
            if (loop && updateMode == UpdateMode.FIXED_UPDATE)
            {
                Run();
            }
        }

        private void LateUpdate()
        {
            if (loop && updateMode == UpdateMode.LATE_UPDATE)
            {
                Run();
            }
        }

        protected override void Run()
        {
            if (!root.IsRunning)
            {
                OnStarted?.Invoke();
                root.Run(callback: this);
            }
        }

        protected override void OnAbort()
        {
            if (IsRunning)
            {
                root.Abort();
                OnAborted?.Invoke();
            }
        }
        void IBehaviourCallback.Invoke(IBehaviourNode node, bool success)
        {
            Return(success);
            OnFinished?.Invoke(success);
        }

        private enum UpdateMode
        {
            UPDATE = 0,
            FIXED_UPDATE = 1,
            LATE_UPDATE = 2
        }

#if UNITY_EDITOR
        public UnityBehaviourNode Editor_GetRoot()
        {
            return root;
        }
#endif
    }
}