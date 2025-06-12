using System;

namespace nicenice.Server.Models
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid DriverId { get; set; }
        public Guid CarId { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal? AdminFee { get; set; }
        public decimal? WeeklyRentalAmount { get; set; }
        public decimal? Deposit { get; set; }
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
        public string? TransactionRef { get; set; }
        public virtual Cars? Car { get; set; }
        public virtual Drivers? Driver { get; set; }

    }
}
