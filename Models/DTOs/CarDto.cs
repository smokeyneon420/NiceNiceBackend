namespace nicenice.Server.Models.DTOs
{
    public class CarDto
    {
        public Guid Id { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public string RegistrationNumber { get; set; } = "";
        public int Year { get; set; }
        public int WeeklyRental { get; set; }
        public string OwnerName { get; set; } = "Owner";
        public string Location { get; set; } = "Unknown Location";
        public string? ImageUrl { get; set; }
        public DateTime Created { get; set; }
        public string Mileage { get; set; } = "N/A";
        public int CarTypeId { get; set; }
        public PricingDto? Pricing { get; set; }
    }
    public class PricingDto
    {
        public int AdminFee { get; set; }
        public int Deposit { get; set; }
        public int MinContractMonths { get; set; }
        public int MaxContractMonths { get; set; }
    }
}
