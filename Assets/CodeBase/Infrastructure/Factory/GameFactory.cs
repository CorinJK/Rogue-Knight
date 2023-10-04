using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.Randomizer;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;
        private readonly IWindowService _windowService;

        // Два свойства - листы
        public List<ISavedProgressReader> progressReaders { get; } = new List<ISavedProgressReader>();      // Те, кто хочет прочитать
        public List<ISavedProgress> progressWriters { get; } = new List<ISavedProgress>();                  // Те, кто хотят записать

        private GameObject HeroGameObject { get; set; }

        public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService, IWindowService windowService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
            _windowService = windowService;
        }

        // Предварительное прогревание ресурсов
        public async Task WarmUp()
        {
            await _assets.Load<GameObject>(AssetsAddress.Loot);
            await _assets.Load<GameObject>(AssetsAddress.Spawner);
        }

        // Создать лут
        public async Task<LootPiece> CreateLoot()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetsAddress.Loot);     // Загружаем прогретый
            LootPiece lootPiece = InstantiateRegistered(prefab)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);

            return lootPiece;
        }

        // Создать героя
        public async Task<GameObject> CreateHero(Vector3 initialPoinPosition)
        {
            HeroGameObject = await InstantiateRegisteredAsync(AssetsAddress.HeroPath, at: initialPoinPosition);  // Берем ссылку
            return HeroGameObject;
        }

        // Создать Hud
        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetsAddress.HudPath);
            hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(_windowService);

            return hud;
        }

        // Создать спавнер
        public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetsAddress.Spawner);     // Загружаем прогретый
            SpawnPoint spawner = InstantiateRegistered(prefab, at)
                .GetComponent<SpawnPoint>();

            spawner.Construct(this);
            spawner.Id = spawnerId;
            spawner.MonsterTypeId = monsterTypeId;
        }

        // Создать монстра
        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(monsterData.PrefabReference);      // Счётчик ссылок создается 

            GameObject prefab = await handle.Task;

            Addressables.Release(handle);           // Счётчик сссылок уменьшается

            GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            IHealth health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max = monsterData.Hp;

            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            // Подключение лута
            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);

            Attack attack = monster.GetComponent<Attack>();
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

            _assets.CleanUp();
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

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        // Инстанциирует с перегрузкой
        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        // Инстанциирует
        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        // Инстанциирует с перегрузкой
        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _assets.Instantiate(prefabPath);
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