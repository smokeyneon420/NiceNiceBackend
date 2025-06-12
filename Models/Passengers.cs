using System.ComponentModel.DataAnnotations.Schema;

namespace nicenice.Server.Models
{
    [Table("Passenegrs", Schema = "Passengers")]
    public class Passengers
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public NiceUser? User { get; set; }
    }
}
