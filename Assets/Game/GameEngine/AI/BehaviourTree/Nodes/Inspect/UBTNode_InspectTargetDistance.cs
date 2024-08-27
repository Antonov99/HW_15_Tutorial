using System.Collections;
using AI.Blackboards;
using AI.BTree;
using Elementary;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Inspect Target Distance»")]
    public sealed class UBTNode_InspectTargetDistance : UnityBehaviourNode_Coroutine, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [BlackboardKey]
        [SerializeField]
        private string targetKey;

        [BlackboardKey]
        [SerializeField]
        private string unitKey;

        [Space]
        [SerializeField]
        private FloatAdapter observePeriod; //0.2f;

        [SerializeField]
        private FloatAdapter visibleDistance; // 3.0f;

        private IComponent_GetPosition unitComponent;

        private IComponent_GetPosition enemyComponent;

        protected override IEnumerator RunRoutine()
        {
            if (!Blackboard.TryGetVariable(unitKey, out IEntity unit))
            {
                Return(false);
                yield break;
            }

            if (!Blackboard.TryGetVariable(targetKey, out IEntity enemy)) 
            {
                Return(false);
                yield break;
            }
            
            unitComponent = unit.Get<IComponent_GetPosition>();
            enemyComponent = enemy.Get<IComponent_GetPosition>();

            yield return HandleDistance();
        }

        private IEnumerator HandleDistance()
        {
            var period = new WaitForSeconds(observePeriod.Current);
            while (true)
            {
                yield return period;
                if (!IsDistanceReached())
                {
                    Return(false);
                    yield break;
                }
            }
        }
        
        private bool IsDistanceReached()
        {
            var unitPosition = unitComponent.Position;
            var targetPosition = enemyComponent.Position;

            var distanceVector = targetPosition - unitPosition;
            return distanceVector.sqrMagnitude <= Mathf.Pow(visibleDistance.Current, 2);
        }
    }
}