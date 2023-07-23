using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    // Может только читать прогресс
    public interface ISavedProgressReader
    {
        // Загрузить прогресс
        void LoadProgress(PlayerProgress progress);
    }

    public interface ISavedProgress : ISavedProgressReader
    {
        // Дописать что то в прогресс
        void UpdateProgress(PlayerProgress progress);
    }
}