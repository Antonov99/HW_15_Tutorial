using System.Collections.Generic;
using AI.Blackboards;
using AI.BTree;
using GameSystem;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "Behaviour Tree Aborter «By Blackboard»")]
    public sealed class UBehaviourTreeAborter_ByBlackboard : MonoBehaviour,
        IBehaviourTreeInjective,
        IBlackboardInjective,
        IGameStartElement,
        IGameFinishElement
    {
        public IBehaviourTree Tree { private get; set; }

        public IBlackboard Blackboard { private get; set; }

        [BlackboardKey]
        [SerializeField]
        private List<string> addBlackboardKeys;

        [BlackboardKey]
        [SerializeField]
        private List<string> removeBlackboardKeys;

        private bool abortRequired;

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (abortRequired)
            {
                Tree.Abort();
                abortRequired = false;
            }
        }

        void IGameStartElement.StartGame()
        {
            Blackboard.OnVariableAdded += OnVariableAdded;
            Blackboard.OnVariableRemoved += OnVariableRemoved;
            enabled = true;
        }

        void IGameFinishElement.FinishGame()
        {
            Blackboard.OnVariableAdded -= OnVariableAdded;
            Blackboard.OnVariableRemoved -= OnVariableRemoved;
            enabled = false;
        }

        private void OnVariableAdded(string key, object value)
        {
            if (addBlackboardKeys.Contains(key))
            {
                abortRequired = true;
            }
        }

        private void OnVariableRemoved(string key, object value)
        {
            if (removeBlackboardKeys.Contains(key))
            {
                abortRequired = true;
            }
        }
    }
}