using GameSystem;

namespace Lessons.I.Architecture.Lesson_SaveLoad
{
    public abstract class SaveLoader<TService, TData> : ISaveLoader
    {
        public void SaveGame(GameContext gameContext, IGameRepository gameRepository)
        {
            TService service = gameContext.GetService<TService>();
            TData data = ConvertToData(service);
            gameRepository.SetData(data);
        }

        public void LoadGame(GameContext gameContext, IGameRepository gameRepository)
        {
            TService service = gameContext.GetService<TService>();
            
            if (gameRepository.TryGetData(out TData data))
            {
                SetupData(service, data);
            }
            else
            {
                SetupDefaultData(service);
            }
        }
        
        protected abstract TData ConvertToData(TService service);
        protected abstract void SetupData(TService service, TData data);

        protected virtual void SetupDefaultData(TService service) { }
    }
}