using nicenice.Server.Models;

namespace nicenice.Server.Data.Repository
{
    public interface ICommonRepo
    {
        Task<List<CarColors>> GetAllCarColors();
        //Task<List<Cars>> GetAllCars();
        Task<PaginatedResult<Cars>> GetAllCars(int pageNumber, int pageSize, string searchTerm = null);
        Task<List<CarTypes>> GetAllCarTypes();
        //Task<List<Drivers>> GetAllDrivers();
        Task<PaginatedResult<Drivers>> GetAllDrivers(int pageNumber, int pageSize, string searchTerm = null);
        Task<List<Gender>> GetAllGender();
        //Task<List<Owner>> GetAllOwners();
        Task<PaginatedResult<Owner>> GetAllOwners(int pageNumber, int pageSize, string searchTerm = null);
        Task<List<Platform>> GetAllPlatform();
        Task<List<Provinces>> GetAllProvinces();
        Task<List<Town>> GetAllTowns();
        Task<PaginatedResult<Users>> GetAllUsers(int pageNumber, int pageSize, string searchTerm = null);
        Task<Drivers> GetFirstDriver(Guid driverId);
        Task<Owner> GetFirstOwner(Guid ownerId);
        Task UpdateFirstDriverProfile(Drivers model);
        Task UpdateFirstOwnerProfile(Owner model);
    }
}
