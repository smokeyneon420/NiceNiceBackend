using nicenice.Server.Data.Repository;
using nicenice.Server.NiceNiceDb;
using nicenice.Server.Repository;

namespace nicenice.Server.Data.UnitOfWorks
{
    public class RepoUnitOfWorks : IRepoUnitOfWorks
    {
        private readonly NiceNiceDbContext _niceNiceDbContext;
        private IOwnerRepo _ownerRepo;
        private ICommonRepo _commonRepo;
        private IDriverRepo _driverRepo;
        private IChatRepo _chatRepo;
        private IPaymentsRepo _paymentsRepo;
        private IInvoiceRepo _invoiceRepo;
        private IPassengerDriverRepo _passengerDriverRepo;
        public RepoUnitOfWorks(NiceNiceDbContext niceNiceDbContext, IOwnerRepo ownerRepo, ICommonRepo commonRepo, IDriverRepo driverRepo, IChatRepo chatRepo, IPaymentsRepo paymentsRepo, IInvoiceRepo invoiceRepo, IPassengerDriverRepo passengerDriverRepo)
        {
            _niceNiceDbContext = niceNiceDbContext;
            _ownerRepo = ownerRepo;
            _commonRepo = commonRepo;
            _driverRepo = driverRepo;
            _chatRepo = chatRepo;
            _paymentsRepo = paymentsRepo;
            _invoiceRepo = invoiceRepo;
            _passengerDriverRepo = passengerDriverRepo;
        }
        public IOwnerRepo ownerRepo => _ownerRepo ??= new OwnerRepo(_niceNiceDbContext);
        public ICommonRepo commonRepo => _commonRepo ??= new CommonRepo(_niceNiceDbContext);
        public IDriverRepo driverRepo => _driverRepo ??= new DriverRepo(_niceNiceDbContext);
        public IChatRepo chatRepo => _chatRepo ??= new ChatRepo(_niceNiceDbContext);
        public IPaymentsRepo paymentsRepo => _paymentsRepo ??= new PaymentsRepo(_niceNiceDbContext);
        public IInvoiceRepo invoiceRepo => _invoiceRepo ??= new InvoiceRepo(_niceNiceDbContext);
        public IPassengerDriverRepo passengerDriverRepo => _passengerDriverRepo ??= new PassengerDriverRepo(_niceNiceDbContext);
        public async Task<int> SaveChangesAsync()
        { return await _niceNiceDbContext.SaveChangesAsync(); }
        public void Dispose()
        {
            _niceNiceDbContext.Dispose();
        }
    }
}
