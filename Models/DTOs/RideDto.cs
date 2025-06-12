public class RideDto
{
    public Guid RideId { get; set; }
    public string? PickupLocation { get; set; }
    public string? DropoffLocation { get; set; }
    public double EstimatedFare { get; set; }
    public RideStatus Status { get; set; }
    public DateTime RequestedAt { get; set; }
}