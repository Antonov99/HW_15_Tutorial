using AI.Blackboards;
using AI.BTree;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Assign Position» (From Entity)")]
    public sealed class UBTNode_Entity_AssignPosition : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [BlackboardKey]
        [SerializeField]
        private string entityKey;

        [BlackboardKey]
        [SerializeField]
        private string resultPositionKey;
    
        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(entityKey, out IEntity entity))
            {
                Debug.LogWarning("Entity is not found!");
                Return(false);
                return;
            }
            
            var targetTransform = entity.Get<IComponent_GetPosition>();
            var targetPosition = targetTransform.Position;
            Blackboard.ReplaceVariable(resultPositionKey, targetPosition);
            Return(true);
        }
    }
}