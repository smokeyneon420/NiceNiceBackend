public class Message
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string? MessageText { get; set; }
    public DateTime SentAt { get; set; }
}