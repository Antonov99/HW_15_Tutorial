using AI.Blackboards;
using AI.BTree;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Inspect Target Destroy»")]
    public sealed class UBTNode_InspectTargetDestroy : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [BlackboardKey]
        [SerializeField]
        private string targetKey;

        private IComponent_OnDestroyed<DestroyArgs> targetComponent;

        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(targetKey, out IEntity target))
            {
                Return(false);
                return;
            }

            targetComponent = target.Get<IComponent_OnDestroyed<DestroyArgs>>();
            targetComponent.OnDestroyed += OnDestroyed;
        }

        protected override void OnDispose()
        {
            if (targetComponent != null)
            {
                targetComponent.OnDestroyed -= OnDestroyed;
                targetComponent = null;
            }
        }

        private void OnDestroyed(DestroyArgs destroyArgs)
        {
            Return(false);
        }
    }
}