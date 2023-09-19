using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Randomizer;
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
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;

        // Два свойства - листы
        public List<ISavedProgressReader> progressReaders { get; } = new List<ISavedProgressReader>();      // Те, кто хочет прочитать
        public List<ISavedProgress> progressWriters { get; } = new List<ISavedProgress>();                  // Те, кто хотят записать

        private GameObject HeroGameObject { get; set; }

        public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
        }
        
        // Создать лут
        public LootPiece CreateLoot()
        {
            LootPiece lootPiece = InstantiateRegistered(AssetPath.Loot)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);
            //lootPiece.Id = id;

            return lootPiece;
        }

        // Создать героя
        public GameObject CreateHero(GameObject at)
        {
            HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);  // Берем ссылку
            return HeroGameObject;
        }

        // Создать Hud
        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.WorldData);
            
            return hud;
        }

        // Создать монстра
        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);
            GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

            var health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max = monsterData.Hp;

            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            // Подключение лута
            var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);

            var attack = monster.GetComponent<Attack>();
            attack.Construct(HeroGameObject.transform);
            attack.Damage = monsterData.Damage;
            attack.CLeavage = monsterData.CLeavage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;

            monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);

            return monster;
        }

        // Создать спавнер
        public void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            EnemySpawner spawner = InstantiateRegistered(AssetPath.Spawner, at)
                .GetComponent<EnemySpawner>();

            spawner.Id = spawnerId;
            spawner.MonsterTypeId = monsterTypeId;
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