using Application.Interfaces;
using MediatR;

namespace Infra
{
    public abstract class BaseBroker : IBroker
    {
        private readonly IMediator _mediator;

        public BaseBroker(IMediator mediator) => _mediator = mediator;

        public abstract Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) 
            where TEvent : IDomainEvent;

        public virtual Task ReceiveAsync<TEvent, TCommand>(string exchangeName, string queueName, Action<TEvent> @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent<TCommand>
            where TCommand : ICommand =>
            ReceiveAsync<TEvent, TCommand>(exchangeName, string.Empty, queueName, @event, cancellationToken);

        public virtual Task ReceiveAsync<TEvent, TCommand>(string exchangeName, string queueName, CancellationToken cancellationToken = default) 
            where TEvent : IEvent<TCommand>
            where TCommand: ICommand => 
            ReceiveAsync<TEvent, TCommand>(exchangeName, queueName, @event => _mediator.Send(@event.ToCommand(), cancellationToken), cancellationToken);

        public abstract Task ReceiveAsync<TEvent, TCommand>(string exchangeName, string topicName, string queueName, Action<TEvent> action, CancellationToken cancellationToken = default)
            where TEvent : IEvent<TCommand>
            where TCommand : ICommand;

        public virtual Task ReceiveAsync<TEvent, TCommand>(string exchangeName, string topicName, string queueName, CancellationToken cancellationToken = default)
            where TEvent : IEvent<TCommand>
            where TCommand : ICommand =>
            ReceiveAsync<TEvent, TCommand>(exchangeName, topicName, queueName, @event => _mediator.Send(@event.ToCommand(), cancellationToken), cancellationToken);
    }
}
