namespace CodeBase.Infrastructure
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }

    // Общий для двух верхних интерфейс
    public interface IExitableState
    {
        void Exit();
    }
}