using nicenice.Server.Models;
using System.Threading.Tasks;
using nicenice.Server.NiceNiceDb;

namespace nicenice.Server.Repository
{
    public class PaymentsRepo : IPaymentsRepo
    {
        private readonly NiceNiceDbContext _context;

        public PaymentsRepo(NiceNiceDbContext context)
        {
            _context = context;
        }

        public async Task AddPayment(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }
    }
}
