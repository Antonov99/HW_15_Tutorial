using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Hit Points/Hit Points")]
    public sealed class UHitPoints : MonoBehaviour, IHitPoints
    {
        public event Action OnSetuped; 

        public event Action<int> OnCurrentPointsChanged;

        public event Action<int> OnMaxPointsChanged;

        public event Action OnCurrentPointsFull;

        public event Action OnCurrentPointsOver;

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

        [SerializeField]
        private int maxHitPoints;

        [SerializeField]
        private int currentHitPoints;

        [Title("Methods")]
        [GUIColor(0, 1, 0)]
        [Button]
        public void Setup(int current, int max)
        {
            maxHitPoints = max;
            currentHitPoints = Mathf.Clamp(current, 0, maxHitPoints);
            OnSetuped?.Invoke();
        }

        [GUIColor(0, 1, 0)]
        [Button]
        private void SetCurrentPoints(int value)
        {
            value = Mathf.Clamp(value, 0, maxHitPoints);
            currentHitPoints = value;
            OnCurrentPointsChanged?.Invoke(currentHitPoints);

            if (value <= 0)
            {
                OnCurrentPointsOver?.Invoke();
            }

            if (value >= maxHitPoints)
            {
                OnCurrentPointsFull?.Invoke();
            }
        }

        [Button]
        [GUIColor(0, 1, 0)]
        private void SetMaxPoints(int value)
        {
            value = Math.Max(1, value);
            if (currentHitPoints > value)
            {
                currentHitPoints = value;
            }

            maxHitPoints = value;
            OnMaxPointsChanged?.Invoke(value);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            maxHitPoints = Math.Max(1, maxHitPoints);
            currentHitPoints = Mathf.Clamp(currentHitPoints, 1, maxHitPoints);
        }
#endif
    }
}