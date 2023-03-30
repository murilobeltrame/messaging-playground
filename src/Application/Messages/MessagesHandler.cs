using Application.Interfaces;
using Application.Messages.Command;
using Application.Messages.Events;

namespace Application.Messages
{
    public class MessagesHandler : 
        ICommandHandler<CreateMessageCommand>,
        ICommandHandler<ProcessMessageCommand>
    {
        private readonly IBroker _broker;

        public MessagesHandler(IBroker broker) => _broker = broker;

        public Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity();
            var @event = MessageCreatedEvent.FromEntity(entity);
            return _broker.PublishAsync(@event, cancellationToken);
        }

        public Task Handle(ProcessMessageCommand request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity();
            Console.WriteLine(entity);
            return Task.CompletedTask;
        }
    }
}
