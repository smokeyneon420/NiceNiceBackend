using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Data.UnitOfWorks;
using nicenice.Server.Models;
using System;
using System.Threading.Tasks;

namespace nicenice.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IRepoUnitOfWorks _repo;

        public PaymentsController(IRepoUnitOfWorks repo)
        {
            _repo = repo;
        }

        [HttpPost("record")]
        public async Task<IActionResult> RecordPayment([FromBody] PaymentRequest request)
        {
            if (request == null || request.DriverId == Guid.Empty || request.CarId == Guid.Empty || request.WeeklyRentalAmount <= 0)
            {
                return BadRequest("Invalid payment data.");
            }

            var paidAt = DateTime.UtcNow;
            var amountPaid = 
            Convert.ToDecimal(request.WeeklyRentalAmount) +
            Convert.ToDecimal(request.AdminFee) +
            Convert.ToDecimal(request.Deposit);

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                DriverId = request.DriverId,
                CarId = request.CarId,
                AmountPaid = amountPaid,
                TransactionRef = request.TransactionRef,
                PaidAt = paidAt,
                AdminFee = request.AdminFee,
                WeeklyRentalAmount = request.WeeklyRentalAmount,
                Deposit = request.Deposit
            };

            await _repo.paymentsRepo.AddPayment(payment);
            await _repo.SaveChangesAsync();

            var car = await _repo.ownerRepo.GetCarById(request.CarId);
            if (car == null)
            {
                return NotFound("Car not found.");
            }

            var invoice = new Invoice
            {
                InvoiceId = Guid.NewGuid(),
                CarId = request.CarId,
                DriverId = request.DriverId,
                OwnerId = car.OwnerId ?? Guid.Empty,
                Amount = amountPaid,
                Status = "Paid",
                DateIssued = paidAt,
                WeekStart = paidAt,
                WeekEnd = paidAt.AddDays(7),
                Description = $"Invoice for rental of car ID: {request.CarId}"
            };

            await _repo.invoiceRepo.CreateInvoice(invoice);
            await _repo.SaveChangesAsync();

            return Ok("Payment recorded and invoice generated.");
        }
    }
}
