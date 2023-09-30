using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        // Зависимости
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;

        // Конструктор
        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, 
            IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();                            // Показать окно загрузки
            _gameFactory.Cleanup();                     // Зачистить коллекции
            _sceneLoader.Load(sceneName, OnLoaded);     // Загрузить сцену
        }

        public void Exit() => 
            _curtain.Hide();                            // Исчезание окна загрузки

        // Загрузка уровня
        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitUIRoot() => 
            _uiFactory.CreateUIRoot();

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.progressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        // Загружение мира: героя и Hud экрана
        private void InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            InitSpawners(levelData);                         // Инициировать спавнеры
            GameObject hero = InitHero(levelData);           // Инициировать героя
            InitHud(hero);                                   // Инициировать hud
            CameraFollow(hero);                              // Подключить камеру
        }

        private void InitSpawners(LevelStaticData levelData)
        {
            // Для каждого спавнера
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
                _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();

            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<HeroHealth>());
        }

        private GameObject InitHero(LevelStaticData levelData) => 
            _gameFactory.CreateHero(levelData.InitialHeroPosition);

        private LevelStaticData LevelStaticData() => 
            _staticData.ForLevel(SceneManager.GetActiveScene().name);   // Берем имя активной сцены

        // Слежение камеры
        private static void CameraFollow(GameObject hero)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);
        }
    }
}