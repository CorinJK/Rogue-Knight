using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";

        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        // Сохранение прогресса
        public void SaveProgress()
        {
            // Опросили всех и записали в прогресс
            foreach (ISavedProgress progressWriter in _gameFactory.progressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

            // Записть в PlayerPrefs
            PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
        }

        // Выгрузить из PlayerPrefs
        public PlayerProgress LoadProgress() => 
            PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();
    }
}