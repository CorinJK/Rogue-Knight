using CodeBase.CameraLogic;
using CodeBase.Logic;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using CodeBase.Infrastructure.Services.PersistentProgress;

namespace CodeBase.Infrastructure.States
{
    internal class LoadLevelState : IPayloadedState<string>
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

        public void Exit()
        {
            _curtain.Hide();
        }

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

        // Загружение героя и Hud экрана
        private void InitGameWorld()
        {
            // Найти объект метку по тегу
            GameObject hero = _gameFactory.CreateHero(at: GameObject.FindWithTag(InitialPointTag));
            _gameFactory.CreateHud();

            // Подключить камеру
            CameraFollow(hero);
        }

        // Слежение камеры
        private void CameraFollow(GameObject hero)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);
        }
    }
}