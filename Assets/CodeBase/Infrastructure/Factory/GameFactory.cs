using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;

        // Два свойства - листы
        public List<ISavedProgressReader> progressReaders { get; } = new List<ISavedProgressReader>();      // Те, кто хочет прочитать
        public List<ISavedProgress> progressWriters { get; } = new List<ISavedProgress>();                  // Те, кто хотят записать

        private GameObject HeroGameObject { get; set; }

        public GameFactory(IAssets assets, IStaticDataService staticData)
        {
            _assets = assets;
            _staticData = staticData;
        }

        // Создать героя
        public GameObject CreateHero(GameObject at)
        {
            HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);  // Берем ссылку
            return HeroGameObject;
        }

        // Создать Hud
        public GameObject CreateHud() => 
            InstantiateRegistered(AssetPath.HudPath);

        // Создать монстра
        public GameObject CreateMonster(MonsterTypeId TypeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(TypeId);
            GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

            var health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max = monsterData.Hp;

            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<AgentMoveToHero>().Construct(HeroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            var attack = monster.GetComponent<Attack>();
            attack.Construct(HeroGameObject.transform);
            attack.Damage = monsterData.Damage;
            attack.CLeavage = monsterData.CLeavage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;

            monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);

            return monster;
        }

        // Зачищать коллекции
        public void Cleanup()
        {
            progressReaders.Clear();
            progressWriters.Clear();
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
    }
}