using Application.Interfaces;

namespace Application.Messages.Command
{
    public record ProcessMessageCommand(string Text, DateTime CreatedAt) : ICommand
    {
        internal Message ToEntity()
        {
            var message = new Message(Text);
            message = message.AtDate(CreatedAt);
            return message;
        }
    }
}
