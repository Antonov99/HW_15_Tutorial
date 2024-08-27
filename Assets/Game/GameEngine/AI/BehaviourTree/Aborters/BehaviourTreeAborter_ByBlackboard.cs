using System;
using System.Collections.Generic;
using AI.Blackboards;
using AI.BTree;
using Declarative;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [Serializable]
    public sealed class BehaviourTreeAborter_ByBlackboard :
        IEnableListener,
        IDisableListener,
        IUpdateListener
    {
        public IBehaviourTree tree;

        public IBlackboard blackboard;
        
        [BlackboardKey]
        [SerializeField]
        public List<string> blackboardKeys;

        private bool abortRequired;

        void IEnableListener.OnEnable()
        {
            blackboard.OnVariableAdded += OnVariableChanged;
            blackboard.OnVariableRemoved += OnVariableChanged;
        }

        void IUpdateListener.Update(float deltaTime)
        {
            if (abortRequired)
            {
                tree.Abort();
                abortRequired = false;
            }
        }

        void IDisableListener.OnDisable()
        {
            blackboard.OnVariableAdded -= OnVariableChanged;
            blackboard.OnVariableRemoved -= OnVariableChanged;
        }
        
        private void OnVariableChanged(string key, object value)
        {
            if (blackboardKeys.Contains(key))
            {
                abortRequired = true;
            }
        }
    }
}