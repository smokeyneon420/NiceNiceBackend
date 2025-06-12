using System;

namespace nicenice.Server.Models
{
    public class PaymentRequest
    {
        public Guid DriverId { get; set; }
        public Guid CarId { get; set; }
        public decimal AdminFee { get; set; }
        public decimal WeeklyRentalAmount { get; set; }
        public decimal? Deposit { get; set; }
        public string? TransactionRef { get; set; }
    }
}
