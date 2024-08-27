using System;
using Elementary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class HitPoints : IHitPoints
    {
        public event Action OnSetuped;

        public event Action<int> OnCurrentPointsChanged;

        public event Action<int> OnMaxPointsChanged;

        private ActionComposite setupActions;

        private ActionComposite<int> onCurrentPointsChanged;

        private ActionComposite<int> onMaxPointsChanged;

        public int Current
        {
            get { return currentHitPoints; }
            set { SetCurrentPoints(value); }
        }

        public int Max
        {
            get { return maxHitPoints; }
            set { SetMaxPoints(value); }
        }

        [SerializeField, OnValueChanged("SetMaxPoints")]
        private int maxHitPoints;

        [SerializeField, OnValueChanged("SetCurrentPoints")]
        private int currentHitPoints;

        [Title("Methods")]
        [GUIColor(0, 1, 0)]
        [Button]
        public void Setup(int current, int max)
        {
            maxHitPoints = max;
            currentHitPoints = Mathf.Clamp(current, 0, maxHitPoints);

            setupActions?.Do();
            OnSetuped?.Invoke();
        }

        private void SetCurrentPoints(int value)
        {
            value = Mathf.Clamp(value, 0, maxHitPoints);
            currentHitPoints = value;

            onCurrentPointsChanged?.Do(value);
            OnCurrentPointsChanged?.Invoke(value);
        }

        private void SetMaxPoints(int value)
        {
            value = Math.Max(1, value);
            if (currentHitPoints > value)
            {
                currentHitPoints = value;
            }

            maxHitPoints = value;

            onMaxPointsChanged?.Do(value);
            OnMaxPointsChanged?.Invoke(value);
        }

        public IAction<int> AddCurrentListener(Action<int> action)
        {
            var actionDelegate = new ActionDelegate<int>(action);
            onCurrentPointsChanged += actionDelegate;
            return actionDelegate;
        }

        public IAction<int> AddMaxListener(Action<int> action)
        {
            var actionDelegate = new ActionDelegate<int>(action);
            onMaxPointsChanged += actionDelegate;
            return actionDelegate;
        }

        public IAction AddSetupListener(Action action)
        {
            var actionDelegate = new ActionDelegate(action);
            setupActions += actionDelegate;
            return actionDelegate;
        }

        public void AddCurrentListener(IAction<int> action)
        {
            onCurrentPointsChanged += action;
        }

        public void AddMaxListener(IAction<int> action)
        {
            onMaxPointsChanged += action;
        }

        public void AddSetupListener(IAction action)
        {
            setupActions += action;
        }

        public void RemoveCurrentListener(IAction<int> action)
        {
            onCurrentPointsChanged -= action;
        }

        public void RemoveMaxListener(IAction<int> action)
        {
            onMaxPointsChanged -= action;
        }

        public void RemoveSetupListener(IAction action)
        {
            setupActions -= action;
        }
    }
}