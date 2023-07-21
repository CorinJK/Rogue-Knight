using CodeBase.CameraLogic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    internal class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";

        // Зависимости
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private Camera _camera;

        // Конструктор
        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        // Загружение героя и Hud экрана
        private void OnLoaded()
        {
            // Найти объект метку по тегу
            GameObject hero = _gameFactory.CreateHero(at: GameObject.FindWithTag(InitialPointTag));
            _gameFactory.CreateHud();

            // Подключить камеру
            CameraFollow(hero);

            _stateMachine.Enter<GameLoopState>();
        }

        // Слежение камеры
        private void CameraFollow(GameObject hero)
        {
            _camera = Camera.main;
            _camera.GetComponent<CameraFollow>().Follow(hero);
        }
    }
}