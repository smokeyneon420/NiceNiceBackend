namespace nicenice.Server.Models.DTOs
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; } 
        public bool IsUnlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
