using System.ComponentModel.DataAnnotations;

public class PassengerRequestDto
{
    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? ContactNumber { get; set; }

    [Required]
    public Guid UserId { get; set; }
}
