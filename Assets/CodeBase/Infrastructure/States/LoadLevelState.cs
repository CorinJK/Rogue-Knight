using CodeBase.CameraLogic;
using CodeBase.Logic;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI;
using CodeBase.Hero;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";

        // Зависимости
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;

        // Конструктор
        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
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
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.progressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        // Загружение мира: героя и Hud экрана
        private void InitGameWorld()
        {
            // Найти объект метку по тегу
            GameObject hero = InitHero();
            InitHud(hero);

            // Подключить камеру
            CameraFollow(hero);
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();

            hud.GetComponentInChildren<ActorUI>()
                .Constract(hero.GetComponent<HeroHealth>());
        }

        private GameObject InitHero() => 
            _gameFactory.CreateHero(at: GameObject.FindWithTag(InitialPointTag));

        // Слежение камеры
        private static void CameraFollow(GameObject hero)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);
        }
    }
}