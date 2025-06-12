public class DriverProfileUpdateDto
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ContactNumber { get; set; }
    public string? Gender { get; set; }
    public int? YearsExperience { get; set; }
}