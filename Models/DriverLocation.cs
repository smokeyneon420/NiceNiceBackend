using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nicenice.Server.Models
{
    public class DriverLocation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Driver")]
        public Guid DriverId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public PassengerDriver? Driver { get; set; }
    }
}
