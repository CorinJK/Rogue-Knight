using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain CurtainPrefab;

        private Game _game;

        private void Awake()
        {
            // Контейнер для всей игры
            _game = new Game(this, Instantiate(CurtainPrefab));
            // Заставить BootstrapState быть точкой входа
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}