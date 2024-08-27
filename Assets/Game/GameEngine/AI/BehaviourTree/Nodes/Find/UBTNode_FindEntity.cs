using AI.Blackboards;
using AI.BTree;
using Entities;
using Game.GameEngine.Entities;
using GameSystem;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Find Entity»")]
    public sealed class UBTNode_FindEntity : UnityBehaviourNode,
        IBlackboardInjective,
        IGameConstructElement
    {
        public IBlackboard Blackboard { private get; set; }

        private EntitiesService entitiesService;

        [SerializeField]
        private ScriptableEntityCondition entityCondition;

        [BlackboardKey]
        [SerializeField]
        private string entityParameterName;
        
        protected override void Run()
        {
            if (entitiesService.FindEntity(entityCondition, out var entity))
            {
                Blackboard.ReplaceVariable(entityParameterName, entity);
                Return(true);
            }
            else
            {
                Return(false);
            }
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            entitiesService = context.GetService<EntitiesService>();
        }
    }
}