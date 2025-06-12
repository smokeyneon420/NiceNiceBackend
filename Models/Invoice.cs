using System;

namespace nicenice.Server.Models
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; } = Guid.NewGuid();
        public Guid CarId { get; set; }
        public Guid DriverId { get; set; }
        public Guid OwnerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public DateTime DateIssued { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Paid";
        public string? Description { get; set; }
    }
}
