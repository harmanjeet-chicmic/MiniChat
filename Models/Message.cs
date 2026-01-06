namespace MiniChat.Server.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Sender { get; set; } = string.Empty;

        public string Receiver { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool IsDelivered { get; set; }
    }
}
