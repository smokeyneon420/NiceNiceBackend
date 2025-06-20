public class FleetCarDto
{
    public int ColorId { get; set; }
    public int CarTypeId { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Name { get; set; }
    public string? Model { get; set; }
    public string? Make { get; set; }
    public int Mileage { get; set; }
    public int PlatformId { get; set; }
    public decimal WeeklyRental { get; set; }
    public DateTime RegistrationExpirydate { get; set; }
    public string? LogoUrl { get; set; }
}
