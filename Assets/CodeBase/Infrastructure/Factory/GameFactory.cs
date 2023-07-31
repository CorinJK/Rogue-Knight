using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;

        public List<ISavedProgressReader> progressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> progressWriters { get; } = new List<ISavedProgress>();

        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }

        // Создать героя
        public GameObject CreateHero(GameObject at) => 
            InstantiateRegistered(AssetPath.HeroPath, at.transform.position);

        // Создать Hud
        public void CreateHud() => 
            InstantiateRegistered(AssetPath.HudPath);

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

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        // Зачищать коллекции
        public void Cleanup()
        {
            progressReaders.Clear();
            progressWriters.Clear();
        }

        // Регистрация
        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                progressWriters.Add(progressWriter);

            progressReaders.Add(progressReader);
        }
    }
}