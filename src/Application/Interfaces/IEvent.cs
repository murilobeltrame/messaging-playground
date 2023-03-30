namespace Application.Interfaces
{
    public interface IEvent
    {
        string? TopicName { get; }
        string ExchangeName { get; }
    }

    public interface IEvent<out TCommand>: IEvent
        where TCommand : ICommand
    {
        TCommand ToCommand();
    }
}
