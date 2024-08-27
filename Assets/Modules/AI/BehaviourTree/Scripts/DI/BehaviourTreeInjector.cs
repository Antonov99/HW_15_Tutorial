using UnityEngine;

namespace AI.BTree
{
    public sealed class BehaviourTreeInjector : MonoBehaviour
    {
        [SerializeField]
        private bool injectOnAwake = true;

        [Space]
        [SerializeField]
        private Transform root;

        [SerializeField]
        private UnityBehaviourTree behaviourTree;

        private void Awake()
        {
            if (injectOnAwake)
            {
                InjectBehaviourTree();
            }
        }

        public void InjectBehaviourTree()
        {
            var injects = root.GetComponentsInChildren<IBehaviourTreeInjective>();
            for (int i = 0, count = injects.Length; i < count; i++)
            {
                var injections = injects[i];
                injections.Tree = behaviourTree;
            }
        }
    }
}