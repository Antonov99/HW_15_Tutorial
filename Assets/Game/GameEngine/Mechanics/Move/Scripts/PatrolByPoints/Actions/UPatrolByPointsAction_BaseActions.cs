using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Melee Combat/Patrol By Points Action «Base Actions»")]
    public sealed class UPatrolByPointsAction_BaseActions : UPatrolByPointsAction
    {
        [SerializeField]
        public MonoAction[] actions;

        public override void Do(PatrolByPointsOperation args)
        {
            for (int i = 0, count = actions.Length; i < count; i++)
            {
                var action = actions[i];
                action.Do();
            }
        }
    }
}