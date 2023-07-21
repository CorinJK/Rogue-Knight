using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain Curtain;

        private Game _game;

        private void Awake()
        {
            // Контейнер для всей игры
            _game = new Game(this, Curtain);
            // Заставить BootstrapState быть точкой входа
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}