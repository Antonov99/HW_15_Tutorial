using System;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Level/Level Visual Adapter")]
    public sealed class LevelVisualAdapter : MonoBehaviour
    {
        [SerializeField]
        private ULevel levelEngine;

        [SerializeField]
        private Mode mode = Mode.EQUALS;

        [Space]
        [SerializeField]
        private Level[] levels = new Level[0];

        private void OnEnable()
        {
            levelEngine.OnLevelSetuped += OnSetuped;
            levelEngine.OnLevelUp += OnLevelUp;
            levelEngine.OnLevelReset += OnResetLevel;
        }

        private void Start()
        {
            Setup(levelEngine.CurrentLevel);
        }

        private void OnDisable()
        {
            levelEngine.OnLevelSetuped -= OnSetuped;
            levelEngine.OnLevelUp -= OnLevelUp;
            levelEngine.OnLevelReset -= OnResetLevel;
        }

        private void OnSetuped(int level)
        {
            Setup(level);
        }

        private void OnResetLevel(int level)
        {
            LevelUp(level);
        }

        private void OnLevelUp(int level)
        {
            LevelUp(level);
        }

        private void Setup(int level)
        {
            if (mode == Mode.EQUALS)
            {
                for (int i = 0, count = levels.Length; i < count; i++)
                {
                    var levelInfo = levels[i];
                    var isVisible = levelInfo.number == level;
                    levelInfo.visual.SetActive(isVisible);
                }
            }
            else if (mode == Mode.LESS_OR_EQUALS)
            {
                for (int i = 0, count = levels.Length; i < count; i++)
                {
                    var levelInfo = levels[i];
                    var isVisible = levelInfo.number <= level;
                    levelInfo.visual.SetActive(isVisible);
                }
            }
        }

        private void LevelUp(int level)
        {
            if (mode == Mode.EQUALS)
            {
                for (int i = 0, count = levels.Length; i < count; i++)
                {
                    var levelInfo = levels[i];
                    if (levelInfo.number == level)
                    {
                        levelInfo.visual.Activate();
                    }
                    else
                    {
                        levelInfo.visual.SetActive(false);
                    }
                }
            }
            else if (mode == Mode.LESS_OR_EQUALS)
            {
                for (int i = 0, count = levels.Length; i < count; i++)
                {
                    var levelInfo = levels[i];
                    if (levelInfo.number == level)
                    {
                        levelInfo.visual.Activate();
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                levelEngine.OnLevelSetuped -= OnSetuped;
                levelEngine.OnLevelSetuped += OnSetuped;
                Setup(levelEngine.CurrentLevel);
            }
            catch (Exception)
            {
            }
        }
#endif

        [Serializable]
        private struct Level
        {
            [SerializeField]
            public int number;

            [SerializeField]
            public LevelVisualBase visual;
        }

        private enum Mode
        {
            EQUALS = 0,
            LESS_OR_EQUALS = 1
        }
    }
}