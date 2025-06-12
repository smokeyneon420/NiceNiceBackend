using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nicenice.Server.Models
{
    [Table("DriverLocations", Schema = "Passengers")]
    public class DriverLocation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DriverId { get; set; }

        [Required]
        public Guid RideId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("DriverId")]
        public virtual Drivers? Driver { get; set; }

        [ForeignKey("RideId")]
        public virtual Ride? Ride { get; set; }
    }
}