using System;
using System.ComponentModel.DataAnnotations;

namespace nicenice.Server.Models.DTOs
{
    public class DriverLocationUpdateDto
    {
        [Required]
        public Guid DriverId { get; set; }

        [Required]
        public Guid RideId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}