using GameSystem;
using Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Lessons.I.Architecture.Lesson_SaveLoad
{
    public class SaveLoadManager : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        private ISaveLoader[] _saveLoaders;

        private GameRepository _gameRepository;

        [ServiceInject]
        public void Construct(ISaveLoader[] saveLoaders, GameRepository gameRepository)
        {
            _saveLoaders = saveLoaders;
            _gameRepository = gameRepository;
        }

        [Button]
        public void SaveGame()
        {
            var gameContext = FindObjectOfType<GameContext>();
            
            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.SaveGame(gameContext, _gameRepository);
            }
            
            _gameRepository.SaveState();
        }

        [Button]
        public void LoadGame()
        {
            _gameRepository.LoadState();
            
            var gameContext = FindObjectOfType<GameContext>();
            
            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.LoadGame(gameContext, _gameRepository);
            }
        }
    }
}