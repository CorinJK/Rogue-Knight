using CodeBase.Infrastructure.States;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, Logic.LoadingCurtain curtain)
        {
            // Подключить стейт машину
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, AllServices.Container);
        }
    }
}