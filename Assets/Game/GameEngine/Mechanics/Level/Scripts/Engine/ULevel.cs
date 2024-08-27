using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Level/Level")]
    public sealed class ULevel : MonoBehaviour
    {
        public event Action<int> OnLevelUp;

        public event Action<int> OnLevelReset;

        public event Action<int> OnLevelSetuped;

        public event Action<int> OnMaxLevelSetuped;

        public int CurrentLevel
        {
            get { return currentLevel; }
        }

        public int MaxLevel
        {
            get { return maxLevel; }
        }

        [SerializeField]
        private int maxLevel;

        [SerializeField]
        private int currentLevel;

        [Title("Methods")]
        [GUIColor(0, 1, 0)]
        [Button]
        public void LevelUp()
        {
            if (currentLevel + 1 >= maxLevel)
            {
                Debug.LogWarning("Can't level up! Max level reached");
            }

            currentLevel++;
            OnLevelUp?.Invoke(currentLevel);
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void ResetLevel()
        {
            currentLevel = 0;
            OnLevelReset?.Invoke(currentLevel);
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void SetupLevel(int level)
        {
            currentLevel = Mathf.Clamp(level, 0, maxLevel);
            OnLevelSetuped?.Invoke(level);
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void SetupMaxLevel(int level)
        {
            if (currentLevel > level)
            {
                currentLevel = level;
            }

            maxLevel = level;
            OnMaxLevelSetuped?.Invoke(level);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            maxLevel = Math.Max(1, maxLevel);
            currentLevel = Mathf.Clamp(currentLevel, 1, maxLevel);
        }
#endif
    }
}