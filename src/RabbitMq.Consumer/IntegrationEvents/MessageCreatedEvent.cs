using Application.Interfaces;
using Application.Messages.Command;

namespace RabbitMq.Consumer.IntegrationEvents
{
    internal record MessageCreatedEvent(string Text, DateTime CreatedAt) : IEvent<ProcessMessageCommand>
    {
        public string? TopicName => string.Empty;

        public string ExchangeName => nameof(MessageCreatedEvent);

        public ProcessMessageCommand ToCommand() => new(Text, CreatedAt);
    }
}
