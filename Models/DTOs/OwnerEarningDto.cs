public class OwnerEarningDto
{
    public Guid CarId { get; set; }
    public string? CarName { get; set; }
    public decimal TotalEarnings { get; set; }
    public decimal Commission { get; set; }
    public decimal NetIncome { get; set; }
    public int PaymentCount { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public decimal? LatestWeeklyRentalAmount { get; set; }
    public string? DriverName { get; set; }
    public string? DriverSurname { get; set; }

}
