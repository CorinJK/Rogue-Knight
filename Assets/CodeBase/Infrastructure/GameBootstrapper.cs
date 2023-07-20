using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;

        private void Awake()
        {
            // Контейнер для всей игры
            _game = new Game();

            DontDestroyOnLoad(this);
        }
    }
}