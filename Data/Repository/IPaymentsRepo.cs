using System.Threading.Tasks;
using nicenice.Server.Models;

namespace nicenice.Server.Repository
{
    public interface IPaymentsRepo
    {
        Task AddPayment(Payment payment);
    }
}
