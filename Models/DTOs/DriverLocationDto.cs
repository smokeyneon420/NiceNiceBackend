namespace nicenice.Server.Models.DTOs
{
    public class DriverLocationDto
    {
        public Guid DriverId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}