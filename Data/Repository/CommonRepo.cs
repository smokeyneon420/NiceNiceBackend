using Microsoft.EntityFrameworkCore;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;

namespace nicenice.Server.Data.Repository
{
    public class CommonRepo : ICommonRepo
    {
        private readonly NiceNiceDbContext _niceniceDbContext;
        public CommonRepo(NiceNiceDbContext niceniceDbContext)
        {
            _niceniceDbContext = niceniceDbContext;
        }
        public async Task<List<Provinces>> GetAllProvinces()
        {
            return await _niceniceDbContext.Provinces.ToListAsync();
        }
        public async Task<List<Town>> GetAllTowns()
        {
            return await _niceniceDbContext.Town.ToListAsync();
        }
        public async Task<List<Gender>> GetAllGender()
        {
            return await _niceniceDbContext.Genders.ToListAsync();
        }
        public async Task<List<Platform>> GetAllPlatform()
        {
            return await _niceniceDbContext.Platforms.ToListAsync();
        }
        public async Task<List<CarTypes>> GetAllCarTypes()
        {
            return await _niceniceDbContext.CarTypes.ToListAsync();
        }
        public async Task<List<CarColors>> GetAllCarColors()
        {
            return await _niceniceDbContext.CarColors.ToListAsync();
        }
        public async Task<PaginatedResult<Cars>> GetAllCars(int pageNumber, int pageSize, string searchTerm = null)
        {
            // Fetch normal owner cars
            var ownerCarsQuery = _niceniceDbContext.Cars
                .Where(x => string.IsNullOrEmpty(searchTerm) ||
                            x.RegistrationNumber.Contains(searchTerm) ||
                            x.Model.Contains(searchTerm) ||
                            x.Make.Contains(searchTerm));

            // Fetch fleet supplier cars
            var fleetCarsQuery = (from car in _niceniceDbContext.FleetSupplierCars
                      join supplier in _niceniceDbContext.FleetSuppliers
                      on car.FleetSupplierId equals supplier.Id
                      where string.IsNullOrEmpty(searchTerm) ||
                            car.RegistrationNumber.Contains(searchTerm) ||
                            car.Model.Contains(searchTerm) ||
                            car.Make.Contains(searchTerm)
                      select new Cars
                      {
                          Id = car.Id,
                          Model = car.Model,
                          Make = car.Make,
                          RegistrationNumber = car.RegistrationNumber,
                          Year = car.Year,
                          WeeklyRental = car.WeeklyRental.ToString(), // or remove ToString() if decimal
                          Created = car.Created,
                          CarOwnerName = supplier.FleetSupplierName + " - " + supplier.BranchManagerName + " " + supplier.BranchManagerSurname,
                          DriverAssigned = "Unassigned"
                      });

            var allCarsQuery = ownerCarsQuery.Select(x => new Cars
            {
                Id = x.Id,
                Model = x.Model,
                Make = x.Make,
                RegistrationNumber = x.RegistrationNumber,
                Year = x.Year,
                WeeklyRental = x.WeeklyRental,
                Created = x.Created,
                CarOwnerName = _niceniceDbContext.Owners
                                .Where(o => o.Id == x.OwnerId)
                                .Select(o => o.FirstName + " " + o.Surname)
                                .FirstOrDefault(),
                DriverAssigned = _niceniceDbContext.Payments
                                .Where(p => p.CarId == x.Id)
                                .OrderByDescending(p => p.PaidAt)
                                .Join(_niceniceDbContext.Drivers,
                                        p => p.DriverId,
                                        d => d.Id,
                                        (p, d) => d.FirstName + " " + d.Surname)
                                .FirstOrDefault()
            });

            var combinedCars = await allCarsQuery.Concat(fleetCarsQuery)
                .OrderByDescending(x => x.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await ownerCarsQuery.CountAsync() + await fleetCarsQuery.CountAsync();

            return new PaginatedResult<Cars>(combinedCars, totalCount);
        }

        public async Task<PaginatedResult<Drivers>> GetAllDrivers(int pageNumber, int pageSize, string searchTerm = null)
        {
            int count = await _niceniceDbContext.Drivers
                .Where(x => string.IsNullOrEmpty(searchTerm) ||
                            x.FirstName.Contains(searchTerm) ||
                            x.Surname.Contains(searchTerm) ||
                            x.Email.Contains(searchTerm))
                .CountAsync();

            var drivers = await _niceniceDbContext.Drivers
                .Where(x => string.IsNullOrEmpty(searchTerm) ||
                            x.FirstName.Contains(searchTerm) ||
                            x.Surname.Contains(searchTerm) ||
                            x.Email.Contains(searchTerm))
                .OrderByDescending(x => x.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var driver in drivers)
            {
                var car = await _niceniceDbContext.Payments
                    .Where(p => p.DriverId == driver.Id)
                    .OrderByDescending(p => p.PaidAt)
                    .Select(p => p.CarId)
                    .FirstOrDefaultAsync();

                var carData = await _niceniceDbContext.Cars.FirstOrDefaultAsync(c => c.Id == car);
                driver.CarAssigned = carData != null ? $"{carData.Make} {carData.Model}" : "Not Assigned";
            }

            return new PaginatedResult<Drivers>(drivers, count);
        }
        public async Task<PaginatedResult<Owner>> GetAllOwners(int pageNumber, int pageSize, string searchTerm = null)
        {
            int count = await _niceniceDbContext.Owners
                .Where(x =>
                    string.IsNullOrEmpty(searchTerm) ||
                    x.FirstName.Contains(searchTerm) ||
                    x.Surname.Contains(searchTerm) ||
                    x.Email.Contains(searchTerm))
                .CountAsync();

            var owners = await _niceniceDbContext.Owners
                .Where(x =>
                    string.IsNullOrEmpty(searchTerm) ||
                    x.FirstName.Contains(searchTerm) ||
                    x.Surname.Contains(searchTerm) ||
                    x.Email.Contains(searchTerm))
                .OrderByDescending(x => x.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var owner in owners)
            {
                owner.NumberOfCars = await _niceniceDbContext.Cars.CountAsync(c => c.OwnerId == owner.Id);
                owner.NumberOfDrivers = await _niceniceDbContext.Invoices
                    .Where(i => i.OwnerId == owner.Id)
                    .Select(i => i.DriverId)
                    .Distinct()
                    .CountAsync();
            }

            return new PaginatedResult<Owner>(owners, count);
        }
        
        public async Task<PaginatedResult<Users>> GetAllUsers(int pageNumber, int pageSize, string searchTerm = null)
        {
            // Get the total count of users, applying the search filter if provided
            int count = await _niceniceDbContext.Users
                .Where(x => string.IsNullOrEmpty(searchTerm) ||
                            x.FirstName.Contains(searchTerm) ||
                            x.LastName.Contains(searchTerm) ||
                            x.Email.Contains(searchTerm))
                .CountAsync();

            // Fetch the users, applying the search filter, sorting, and pagination
            var users = await _niceniceDbContext.Users
                .Where(x => string.IsNullOrEmpty(searchTerm) ||
                            x.FirstName.Contains(searchTerm) ||
                            x.LastName.Contains(searchTerm) ||
                            x.Email.Contains(searchTerm))
                .Select(x => new Users
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    DateOfCreation = x.DateOfCreation // Make this nullable
                })
                .OrderByDescending(x => x.DateOfCreation)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            //return PaginatedResult(users, count);
            return new PaginatedResult<Users>(users, count);
        }

        public async Task<Drivers> GetFirstDriver(Guid driverId)
        {
            return await _niceniceDbContext.Drivers.FirstOrDefaultAsync(x => x.Id == driverId);
        }
        public async Task<Owner> GetFirstOwner(Guid ownerId)
        {
            return await _niceniceDbContext.Owners.FirstOrDefaultAsync(x => x.Id == ownerId);
        }
        public async Task UpdateFirstDriverProfile(Drivers model)
        {
            var driver = await _niceniceDbContext.Set<Drivers>().FirstOrDefaultAsync(d => d.Id == model.Id);


            if (driver != null)
            {
                // Update only the fields that have a value in the updatedModel
                if (!string.IsNullOrEmpty(model.YearsExperience))
                    driver.YearsExperience = model.YearsExperience;

                if (!string.IsNullOrEmpty(model.FirstName))
                    driver.FirstName = model.FirstName;

                if (!string.IsNullOrEmpty(model.Surname))
                    driver.Surname = model.Surname;

                if (!string.IsNullOrEmpty(model.Contact))
                    driver.Contact = model.Contact;

                if (!string.IsNullOrEmpty(model.CriminalRecord))
                    //if (model.CriminalRecord.HasValue)
                    driver.CriminalRecord = model.CriminalRecord;
                if (!string.IsNullOrEmpty(model.InsuranceClaim))
                    //if (model.InsuranceClaim.HasValue)
                    driver.InsuranceClaim = model.InsuranceClaim;

                if (model.NumberOfClaims.HasValue)
                    driver.NumberOfClaims = model.NumberOfClaims;

                if (!string.IsNullOrEmpty(model.Gender))
                    driver.Gender = model.Gender;

                if (model.BuildingNumber.HasValue)
                    driver.BuildingNumber = model.BuildingNumber;

                if (model.ProvinceId.HasValue)
                    driver.ProvinceId = model.ProvinceId;

                if (!string.IsNullOrEmpty(model.Surbub))
                    driver.Surbub = model.Surbub;

                if (model.TownId.HasValue)
                    driver.TownId = model.TownId;

                if (!string.IsNullOrEmpty(model.Street))
                    driver.Street = model.Street;

                //// Update the byte arrays for file uploads
                //if (model.ProfilephotoUrl != null && model.ProfilephotoUrl.Length > 0)
                //    driver.ProfilephotoUrl = model.ProfilephotoUrl;

                //if (model.IdentityDocumentUrl != null && model.IdentityDocumentUrl.Length > 0)
                //    driver.IdentityDocumentUrl = model.IdentityDocumentUrl;

                //if (model.LicenseCopyUrl != null && model.LicenseCopyUrl.Length > 0)
                //    driver.LicenseCopyUrl = model.LicenseCopyUrl;

                //if (model.FingerPrintReportUrl != null && model.FingerPrintReportUrl.Length > 0)
                //    driver.FingerPrintReportUrl = model.FingerPrintReportUrl;

                //if (model.CurrentPlatformScreenshot != null && model.CurrentPlatformScreenshot.Length > 0)
                //    driver.CurrentPlatformScreenshot = model.CurrentPlatformScreenshot;

                //if (model.SelfiePic != null && model.SelfiePic.Length > 0)
                //    driver.SelfiePic = model.SelfiePic;

                //if (model.PassportUrl != null && model.PassportUrl.Length > 0)
                //    driver.PassportUrl = model.PassportUrl;

                //if (model.PermitOrAsylumLetterUrl != null && model.PermitOrAsylumLetterUrl.Length > 0)
                //    driver.PermitOrAsylumLetterUrl = model.PermitOrAsylumLetterUrl;

                //if (model.BankConfirmationLetterUrl != null && model.BankConfirmationLetterUrl.Length > 0)
                //    driver.BankConfirmationLetterUrl = model.BankConfirmationLetterUrl;

                //if (model.ProofOfResidenceOrBankStatementUrl != null && model.ProofOfResidenceOrBankStatementUrl.Length > 0)
                //    driver.ProofOfResidenceOrBankStatementUrl = model.ProofOfResidenceOrBankStatementUrl;

                // Update the Updated timestamp
                //driver.Updated = DateTime.Now;

                // Save changes to the database
                await _niceniceDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Driver with email {model.Email} not found.");
            }
        }

        public async Task UpdateFirstOwnerProfile(Owner model)
        {
            // Fetch the owner entity by email
            var owner = await _niceniceDbContext.Owners.FirstOrDefaultAsync(o => o.Id == model.Id);

            if (owner != null)
            {
                // Update only the fields that have a value in the updatedModel
                if (!string.IsNullOrEmpty(model.FirstName))
                    owner.FirstName = model.FirstName;

                if (!string.IsNullOrEmpty(model.Surname))
                    owner.Surname = model.Surname;

                if (!string.IsNullOrEmpty(model.Contact))
                    owner.Contact = model.Contact;

                if (!string.IsNullOrEmpty(model.Telephone))
                    owner.Telephone = model.Telephone;

                if (model.IdNumber.HasValue)
                    owner.IdNumber = model.IdNumber;

                if (model.IsVerified.HasValue)
                    owner.IsVerified = model.IsVerified;

                if (!string.IsNullOrEmpty(model.Gender))
                    owner.Gender = model.Gender;

                if (model.BuildingNumber.HasValue)
                    owner.BuildingNumber = model.BuildingNumber;

                if (model.ProvinceId.HasValue)
                    owner.ProvinceId = model.ProvinceId;

                if (!string.IsNullOrEmpty(model.Surbub))
                    owner.Surbub = model.Surbub;

                if (model.TownId.HasValue)
                    owner.TownId = model.TownId;

                if (!string.IsNullOrEmpty(model.Street))
                    owner.Street = model.Street;
                if (!string.IsNullOrEmpty(model.InsuranceClaim))
                    //if (model.InsuranceClaim.HasValue)
                    owner.InsuranceClaim = model.InsuranceClaim;

                if (model.NumberOfClaims.HasValue)
                    owner.NumberOfClaims = model.NumberOfClaims;

                //// Update byte arrays for file fields
                //if (model.IdentityDocument != null && model.IdentityDocument.Length > 0)
                //    owner.IdentityDocument = model.IdentityDocument;

                //if (model.VehicleRegistrationDocuments != null && model.VehicleRegistrationDocuments.Length > 0)
                //    owner.VehicleRegistrationDocuments = model.VehicleRegistrationDocuments;

                //if (model.VehicleLicenseDiskAndOperatingCard != null && model.VehicleLicenseDiskAndOperatingCard.Length > 0)
                //    owner.VehicleLicenseDiskAndOperatingCard = model.VehicleLicenseDiskAndOperatingCard;

                //if (model.ProofOfInsurance != null && model.ProofOfInsurance.Length > 0)
                //    owner.ProofOfInsurance = model.ProofOfInsurance;

                //if (model.CarPicture != null && model.CarPicture.Length > 0)
                //    owner.CarPicture = model.CarPicture;

                // Update the Updated timestamp
                //owner.Updated = DateTime.Now;

                // Save changes to the database
                await _niceniceDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Owner with email {model.Email} not found.");
            }
        }
    }
}
