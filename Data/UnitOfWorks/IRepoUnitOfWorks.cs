using nicenice.Server.Data.Repository;
using nicenice.Server.Repository;


namespace nicenice.Server.Data.UnitOfWorks
{
    public interface IRepoUnitOfWorks : IDisposable
    {
        IOwnerRepo ownerRepo { get; }
        ICommonRepo commonRepo { get; }
        IDriverRepo driverRepo { get; }
        IChatRepo chatRepo { get; }
        IPaymentsRepo paymentsRepo { get; }
        IInvoiceRepo invoiceRepo { get; }
        IPassengerDriverRepo passengerDriverRepo { get; }
        Task<int> SaveChangesAsync();
    }
}
