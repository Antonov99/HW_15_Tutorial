using GameSystem;

namespace Lessons.I.Architecture.Lesson_SaveLoad
{
    public interface ISaveLoader
    {
        void SaveGame(GameContext gameContext, IGameRepository gameRepository);
        void LoadGame(GameContext gameContext, IGameRepository gameRepository);
    }
}