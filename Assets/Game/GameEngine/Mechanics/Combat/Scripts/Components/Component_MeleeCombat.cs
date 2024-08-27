using System;
using Elementary;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_MeleeCombat : IComponent_MeleeCombat 
    {
        public event Action<CombatOperation> OnCombatStarted
        {
            add { @operator.OnStarted += value; }
            remove { @operator.OnStarted -= value; }
        }

        public event Action<CombatOperation> OnCombatStopped
        {
            add { @operator.OnStopped += value; }
            remove { @operator.OnStopped -= value; }
        }

        public bool IsCombat
        {
            get { return @operator.IsActive; }
        }

        private readonly IOperator<CombatOperation> @operator;

        public Component_MeleeCombat(IOperator<CombatOperation> @operator)
        {
            this.@operator = @operator;
        }

        public bool CanStartCombat(CombatOperation operation)
        {
            return @operator.CanStart(operation);
        }

        public void StartCombat(CombatOperation operation)
        {
            @operator.DoStart(operation);
        }

        public void StopCombat()
        {
            @operator.Stop();
        }
    }
}