using Application.Interfaces;

namespace Application.Messages.Command
{
    public record CreateMessageCommand(string Text) : ICommand
    {
        internal Message ToEntity() => new(Text);
    }
}
