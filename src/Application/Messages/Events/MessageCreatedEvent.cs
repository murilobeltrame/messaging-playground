using Application.Interfaces;
using Application.Messages.Command;

namespace Application.Messages.Events
{
    public record MessageCreatedEvent(string Text, DateTime CreatedAt): IDomainEvent
    {
        public string? TopicName => null;

        public string ExchangeName => nameof(MessageCreatedEvent);

        internal static MessageCreatedEvent FromEntity(Message entity) => new (entity.Text, entity.CreatedAt);
    }
}
