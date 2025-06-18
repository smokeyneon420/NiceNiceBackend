using System;

namespace nicenice.Server.Models
{
    public class DriverLocation
    {
        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public Guid RideId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public Drivers? Driver { get; set; }
        public Ride? Ride { get; set; }
    }
}
