using System.Text.Json;

namespace Application.Messages
{
    internal class Message
    {
        public Message(string text) => Text = text;

        public string Text { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        public Message AtDate(DateTime dateTime) {
            CreatedAt = dateTime;
            return this;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
