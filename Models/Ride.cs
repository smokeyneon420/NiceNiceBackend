using System.ComponentModel.DataAnnotations.Schema;
using nicenice.Server.Models;

[Table("Rides", Schema = "Passengers")]
public class Ride
{
    public Guid Id { get; set; }
    public Guid PassengerId { get; set; }
    public Guid? DriverId { get; set; }
    public string? PickupLocation { get; set; }
    public string? DropoffLocation { get; set; }
    public double EstimatedFare { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    [Column(TypeName = "int")]
    public RideStatus Status { get; set; }
    public Passengers? Passenger { get; set; }
    public PassengerDriver? Driver { get; set; }
    public Guid? OfferSentToDriverId { get; set; }
    public DateTime? OfferedAt { get; set; }

}

public enum RideStatus
{
    Requested,
    Matched,
    Started,
    Completed,
    Cancelled
}