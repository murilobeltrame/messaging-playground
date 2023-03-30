namespace Application.Interfaces
{
    public interface IBroker
    {
        Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken) 
            where TEvent : IDomainEvent;
        
        Task ReceiveAsync<TEvent, TCommand>(string exchangeName, string topicName, string queueName, Action<TEvent> action, CancellationToken cancellationToken = default) 
            where TEvent : IEvent<TCommand>
            where TCommand: ICommand;

        Task ReceiveAsync<TEvent, TCommand>(string exchangeName, string topicName, string queueName, CancellationToken cancellationToken = default) 
            where TEvent : IEvent<TCommand>
            where TCommand: ICommand;

        Task ReceiveAsync<TEvent, TCommand>(string exchangeName, string queueName, Action<TEvent> action, CancellationToken cancellationToken = default)
            where TEvent : IEvent<TCommand>
            where TCommand : ICommand;

        Task ReceiveAsync<TEvent, TCommand>(string exchangeName, string queueName, CancellationToken cancellationToken = default)
            where TEvent : IEvent<TCommand>
            where TCommand : ICommand;
    }
}
