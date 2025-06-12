using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nicenice.Server.Models
{
    [Table("Cars", Schema = "Passengers")]
    public class PassengersCar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid DriverId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Make { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Model { get; set; }

        [Required]
        [MaxLength(50)]
        public string? VehicleType { get; set; }

        public int Year { get; set; }

        [Required]
        [MaxLength(50)]
        public string? PlateNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
