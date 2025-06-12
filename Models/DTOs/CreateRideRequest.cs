using System.ComponentModel.DataAnnotations;

public class CreateRideRequest
{
    [Required]
    public Guid PassengerId { get; set; }

    [Required]
    [MinLength(3)]
    public string? PickupLocation { get; set; }

    [Required]
    [MinLength(3)]
    public string? DropoffLocation { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Estimated fare must be a positive number.")]
    public double EstimatedFare { get; set; }
}
