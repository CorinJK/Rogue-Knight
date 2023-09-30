using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain, AllServices services)
        {
            // Конструктор: словарь с ключом Type и значением IState
            _states = new Dictionary<Type, IExitableState>
            {
                //Зарегистрировали
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, curtain, services.Single<IGameFactory>(),
                services.Single<IPersistentProgressService>(), services.Single<IStaticDataService>(), services.Single<IUIFactory>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistentProgressService>(), services.Single<ISaveLoadService>()),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };
        }

        // Входим в стате от параметра дженерика с типом стета, куда ходим перевести
        // where является ограничением
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();       // Присвоить новый стате
            state.Enter();                              // Отправиться в стате
        }

        // Enter для ситуации, когда необходимо учитывать доп.параметр
        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();                       // Выйти из предыдущего стета с проверкой
            TState state = GetState<TState>();          // Получить тип - typeof
            _activeState = state;
            return state;
        }

        // DownCast - as
        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}