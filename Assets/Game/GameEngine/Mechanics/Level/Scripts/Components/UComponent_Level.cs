using System;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Level/Component «Level»")]
    public sealed class UComponent_Level : MonoBehaviour,
        IComponent_GetLevel,
        IComponent_SetupLevel,
        IComponent_LevelUp,
        IComponent_GetMaxLevel,
        IComponent_ResetLevel
    {
        public event Action<int> OnLevelUp
        {
            add { levelEngine.OnLevelUp += value; }
            remove { levelEngine.OnLevelUp -= value; }
        }

        public event Action<int> OnLevelSetuped
        {
            add { levelEngine.OnLevelSetuped += value; }
            remove { levelEngine.OnLevelSetuped -= value; }
        }

        public event Action<int> OnLevelReset
        {
            add { levelEngine.OnLevelReset += value; }
            remove { levelEngine.OnLevelReset -= value; }
        }

        public int Level
        {
            get { return levelEngine.CurrentLevel; }
        }

        public int MaxLevel
        {
            get { return levelEngine.MaxLevel; }
        }

        [SerializeField]
        private ULevel levelEngine;

        public void SetupLevel(int level)
        {
            levelEngine.SetupLevel(level);
        }

        public void LevelUp()
        {
            levelEngine.LevelUp();
        }

        public void ResetLevel()
        {
            levelEngine.ResetLevel();
        }
    }
}