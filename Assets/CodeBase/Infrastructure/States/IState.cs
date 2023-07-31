namespace CodeBase.Infrastructure.States
{
    // Интерфейс без параметров
    public interface IState : IExitableState
    {
        void Enter();
    }

    // Интерфейс с доп. параметром
    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }

    // Общий для двух верхних интерфейсов
    public interface IExitableState
    {
        void Exit();
    }
}