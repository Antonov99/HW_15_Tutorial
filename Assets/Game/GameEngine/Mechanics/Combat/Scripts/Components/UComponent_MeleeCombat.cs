using System;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Melee Combat/Component «Melee Combat»")]
    public sealed class UComponent_MeleeCombat : MonoBehaviour, IComponent_MeleeCombat
    {
        public event Action<CombatOperation> OnCombatStarted
        {
            add { combatOperator.OnStarted += value; }
            remove { combatOperator.OnStarted -= value; }
        }

        public event Action<CombatOperation> OnCombatStopped
        {
            add { combatOperator.OnStopped += value; }
            remove { combatOperator.OnStopped -= value; }
        }

        public bool IsCombat
        {
            get { return combatOperator.IsActive; }
        }

        [SerializeField]
        private UCombatOperator combatOperator;

        public bool CanStartCombat(CombatOperation operation)
        {
            return combatOperator.CanStart(operation);
        }

        public void StartCombat(CombatOperation operation)
        {
            combatOperator.DoStart(operation);
        }

        public void StopCombat()
        {
            combatOperator.Stop();
        }
    }
}