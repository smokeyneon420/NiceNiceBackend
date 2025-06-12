using System.Threading.Tasks;
using nicenice.Server.Models;

namespace nicenice.Server.Repository
{
    public interface IInvoiceRepo
    {
        Task CreateInvoice(Invoice invoice);
    }
}
