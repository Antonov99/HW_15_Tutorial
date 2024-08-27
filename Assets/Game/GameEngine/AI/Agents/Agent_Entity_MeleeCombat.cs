using AI.Agents;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    public sealed class Agent_Entity_MeleeCombat : AgentCoroutine
    {
        private IEntity attacker;

        private IEntity target;

        private IComponent_MeleeCombat combatComponent;

        private Coroutine combatCoroutine;

        public Agent_Entity_MeleeCombat()
        {
            SetFramePeriod(new WaitForFixedUpdate());
        }

        public void SetAttacker(IEntity unit)
        {
            if (attacker != null)
            {
                combatComponent.StopCombat();
            }

            attacker = unit;
            combatComponent = unit?.Get<IComponent_MeleeCombat>();
        }

        public void SetTarget(IEntity target)
        {
            if (attacker != null)
            {
                combatComponent.StopCombat();
            }

            this.target = target;
        }

        protected override void OnStop()
        {
            base.OnStop();

            if (combatComponent.IsCombat)
            {
                combatComponent.StopCombat();
            }
        }

        protected override void Update()
        {
            if (attacker != null && target != null)
            {
                StartCombat();
            }
        }

        private void StartCombat()
        {
            if (combatComponent.IsCombat)
            {
                return;
            }

            var combatOperation = new CombatOperation(target);
            combatComponent.StartCombat(combatOperation);
        }
    }
}