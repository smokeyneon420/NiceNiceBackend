using nicenice.Server.Models;
using nicenice.Server.Models.DTOs;

namespace nicenice.Server.Data.Repository
{
    public interface IOwnerRepo
    {
        Task AddOwnerProfile(Owner model);
        Task UpdateOwnerProfile(Owner model);
        //Task<IEnumerable<Owner>> GetOwnerProfileDetails();
        Task<Owner> GetFirstOwnerProfile(string? email);
        Task<int> GetOwnerCount();
        Task AddOwnerIdentityDocuments(OwnerIdentityDocuments model);
        Task AddVehicleRegistrationDocuments(VehicleRegistrationDocuments model);
        Task AddVehicleLicenseDiskAndOperatingCard(VehicleLicenseDiskAndOperatingCard model);
        Task AddCarPictures(CarPictures model);
        Task AddProofOfInsurance(ProofOfInsurance model);
        Task<bool> OwnerIdentityDocumentsExists(Guid ownerId, string fileName);
        Task<bool> VehicleRegistrationDocumentsExists(Guid ownerId, string fileName);
        Task<bool> VehicleLicenseDiskAndOperatingCardExists(Guid ownerId, string fileName);
        Task<bool> ProofOfInsuranceExists(Guid ownerId, string fileName);
        Task<bool> CarPicturesExists(Guid? ownerId, string fileName);
        Task<List<OwnerIdentityDocuments>> GetAllOwnerIdentityDocuments(Guid ownerId);
        Task<List<VehicleRegistrationDocuments>> GetAllVehicleRegistrationDocuments(Guid ownerId);
        Task<List<VehicleLicenseDiskAndOperatingCard>> GetAllVehicleLicenseDiskAndOperatingCard(Guid ownerId);
        Task<List<ProofOfInsurance>> GetAllProofOfInsurance(Guid ownerId);
        Task<OwnerIdentityDocuments> GetOwnerIdentityDocuments(Guid id);
        Task<VehicleRegistrationDocuments> GetVehicleRegistrationDocuments(Guid id);
        Task<VehicleLicenseDiskAndOperatingCard> GetVehicleLicenseDiskAndOperatingCard(Guid id);
        Task<ProofOfInsurance> GetProofOfInsurance(Guid id);
        Task<CarPictures> GetCarPictures(Guid id);
        Task DeleteOwnerIdentityDocuments(Guid id);
        Task DeleteVehicleRegistrationDocuments(Guid id);
        Task DeleteVehicleLicenseDiskAndOperatingCard(Guid id);
        Task DeleteProofOfInsurance(Guid id);
        Task DeleteCarPictures(Guid id);
        Task<List<CarPictures>> GetAllCarPictures(Guid id);
        Task AddCar(Cars model, CancellationToken cancellationToken = default);
        Task UpdateCar(Cars model, CancellationToken cancellationToken = default);
        Task DeleteCar(Guid id, CancellationToken cancellationToken = default);
        Task<List<Cars>> GetCarsListByOwnerId(Guid ownerId);
        Task<Cars> GetCarById(Guid id);
        Task<bool> CarExists(string registrationNumber, CancellationToken cancellationToken = default);
        Task<IEnumerable<CarDto>> GetAllVehicles();
        Task<Guid?> GetOwnerIdByUserId(Guid userId);
        Task<List<OwnerEarningDto>> GetWeeklyEarnings(Guid ownerId);
        IQueryable<Cars> GetQueryableVehicles();
        Task<BankingDetail?> GetBankingDetailByOwnerId(Guid ownerId);
        Task AddBankingDetail(BankingDetail model);
        Task<IEnumerable<object>> GetAllBankingDetails();

    }
}
