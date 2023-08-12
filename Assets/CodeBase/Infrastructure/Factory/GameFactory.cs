using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;

        // Два свойства - листы
        public List<ISavedProgressReader> progressReaders { get; } = new List<ISavedProgressReader>();      // Те, кто хочет прочитать
        public List<ISavedProgress> progressWriters { get; } = new List<ISavedProgress>();                  // Те, кто хотят записать


        public event Action HeroCreated;
        public GameObject HeroGameObject { get; set; }

        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }

        // Создать героя
        public GameObject CreateHero(GameObject at)
        {
            HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);  // Берем ссылку
            HeroCreated?.Invoke();      // Сообщили что герой создался
            return HeroGameObject;
        }

        // Создать Hud
        public GameObject CreateHud() => 
            InstantiateRegistered(AssetPath.HudPath);

        // Зачищать коллекции
        public void Cleanup()
        {
            progressReaders.Clear();
            progressWriters.Clear();
        }

        // Инстанциирует
        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        // Инстанциирует с перегрузкой
        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        // Ищем все компоненты, которые реализуют интерфейс ISavedProgressReader
        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        // Регистрация
        public void Register(ISavedProgressReader progressReader)
        {
            // Если сущности нужно записать данные
            if (progressReader is ISavedProgress progressWriter)
                progressWriters.Add(progressWriter);

            // Добавили в читателей
            progressReaders.Add(progressReader);
        }
    }
}