using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader)
        {
            // Конструктор: словарь с ключом Type и значением IState
            _states = new Dictionary<Type, IExitableState>
            {
                //Зарегистрировали
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader),
            };
        }

        // Входим в стате от параметра дженерика с типом стета, куда ходим перевести
        // where является ограничением
        public void Enter<TState>() where TState : class, IState
        {
            _activeState?.Exit();                       // Выйти из предыдущего стета с проверкой
            IState state = GetState<TState>();          // Получить тип - typeof
            _activeState = state;                       // Присвоить новый стате
            state.Enter();                              // Отправиться в стате
        }

        // Enter для ситуации, когда необходимо учитывать сцену
        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            _activeState?.Exit();
            IPayloadedState<TPayload> state = GetState<TState>();
            _activeState = state;
            state.Enter(payload);
        }

        // Давнкаст as
        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}