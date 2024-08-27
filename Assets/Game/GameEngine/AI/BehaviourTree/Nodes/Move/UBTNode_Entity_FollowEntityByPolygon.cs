using AI.Blackboards;
using AI.BTree;
using Elementary;
using Entities;
using Polygons;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Follow Entity By Polygon» (Entity)")]
    public class UBTNode_Entity_FollowEntityByPolygon : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [Space]
        [SerializeField]
        private FloatAdapter stoppingDistance;

        [SerializeField]
        private FloatAdapter minPointDistance;

        [Space]
        [BlackboardKey]
        [SerializeField]
        private string unitKey;

        [BlackboardKey]
        [SerializeField]
        private string targetKey;

        [BlackboardKey]
        [SerializeField]
        private string surfaceKey;

        private Agent_Entity_FollowEntityByPolygon followAgent;

        private void Awake()
        {
            followAgent = new Agent_Entity_FollowEntityByPolygon();
            followAgent.SetStoppingDistance(stoppingDistance.Current);
            followAgent.SetIntermediateDistance(minPointDistance.Current);
            followAgent.SetCalculatePathPeriod(new WaitForFixedUpdate());
            followAgent.SetCheckTargetReachedPeriod(null);
        }

        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(unitKey, out IEntity unit))
            {
                Return(false);
                return;
            }

            if (!Blackboard.TryGetVariable(targetKey, out IEntity target))
            {
                Return(false);
                return;
            }

            if (!Blackboard.TryGetVariable(surfaceKey, out MonoPolygon polygon))
            {
                Return(false);
                return;
            }

            followAgent.OnTargetReached += OnTargetReached;
            followAgent.SetSurface(polygon);
            followAgent.SetTargetEntity(target);
            followAgent.SetFollowingEntity(unit);
            followAgent.Play();
        }

        private void OnTargetReached(bool isReached)
        {
            if (isReached)
            {
                Return(true);
            }
        }

        protected override void OnDispose()
        {
            followAgent.Stop();
            followAgent.OnTargetReached -= OnTargetReached;
        }
    }
}