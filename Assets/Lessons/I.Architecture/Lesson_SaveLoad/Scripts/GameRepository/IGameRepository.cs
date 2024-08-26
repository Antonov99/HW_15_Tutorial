namespace Lessons.I.Architecture.Lesson_SaveLoad
{
    public interface IGameRepository
    {
        bool TryGetData<T>(out T data);
        void SetData<T>(T data);
    }
}