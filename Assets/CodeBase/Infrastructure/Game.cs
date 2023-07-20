using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public static IInputService InputService;
        public GameStateMachine StateMachine;

        public Game()
        {
            // Подключить стейт машину
            StateMachine = new GameStateMachine();
        }
    }
}