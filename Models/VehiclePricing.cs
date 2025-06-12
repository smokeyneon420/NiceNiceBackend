public class VehiclePricing
{
    public int Id { get; set; }
    public string? VehicleType { get; set; }
    public decimal WeeklyRental { get; set; }
    public decimal Deposit { get; set; }
    public int MinContractMonths { get; set; }
    public int MaxContractMonths { get; set; }
    public decimal AdminFee { get; set; } = 299;
    public decimal HandlingFeePercentage { get; set; } = 5;
}
