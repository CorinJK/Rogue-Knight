using CodeBase.Services.Input;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public static IInputService InputService;
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, Logic.LoadingCurtain curtain)
        {
            // Подключить стейт машину
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain);
        }
    }
}