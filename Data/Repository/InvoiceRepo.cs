using System.Threading.Tasks;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;

namespace nicenice.Server.Repository
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly NiceNiceDbContext _context;

        public InvoiceRepo(NiceNiceDbContext context)
        {
            _context = context;
        }

        public async Task CreateInvoice(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }
    }
}
