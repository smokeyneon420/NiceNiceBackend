using Microsoft.EntityFrameworkCore;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;
using nicenice.Server.Models.DTOs;

namespace nicenice.Server.Data.Repository
{
    public class OwnerRepo : IOwnerRepo
    {
        private readonly NiceNiceDbContext _niceniceDbContext;

        public OwnerRepo(NiceNiceDbContext niceniceDbContext)
        {
            _niceniceDbContext = niceniceDbContext;
        }
        public async Task AddOwnerProfile(Owner model)
        {
            await _niceniceDbContext.Owners.AddAsync(model);
        }

        public async Task UpdateOwnerProfile(Owner model)
        {
            // Fetch the owner entity by email
            var owner = await _niceniceDbContext.Owners.FirstOrDefaultAsync(o => o.Email == model.Email);

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
        public async Task<Owner> GetFirstOwnerProfile(string? email)
        {
            //return await _niceniceDbContext.Set<Owner>().FindAsync(name);
            return await _niceniceDbContext.Owners.FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<int> GetOwnerCount()
        {
            int count = await _niceniceDbContext.Owners.CountAsync();
            return count;
        }
        public async Task AddOwnerIdentityDocuments(OwnerIdentityDocuments model)
        {
            await _niceniceDbContext.OwnerIdentityDocuments.AddAsync(model);
        }
        public async Task<bool> OwnerIdentityDocumentsExists(Guid ownerId, string fileName)
        {
            return await _niceniceDbContext.OwnerIdentityDocuments
                .AnyAsync(p => p.OwnerId == ownerId && p.FileName == fileName);
        }
        public async Task<List<OwnerIdentityDocuments>> GetAllOwnerIdentityDocuments(Guid ownerId)
        {
            return await _niceniceDbContext.OwnerIdentityDocuments.Where(x => x.OwnerId == ownerId).ToListAsync();
        }
        public async Task<OwnerIdentityDocuments> GetOwnerIdentityDocuments(Guid id)
        {
            return await _niceniceDbContext.OwnerIdentityDocuments.FindAsync(id);
        }
        public async Task DeleteOwnerIdentityDocuments(Guid id)
        {
            var file = await _niceniceDbContext.OwnerIdentityDocuments.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.OwnerIdentityDocuments.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddVehicleRegistrationDocuments(VehicleRegistrationDocuments model)
        {
            await _niceniceDbContext.VehicleRegistrationDocuments.AddAsync(model);
        }
        public async Task<bool> VehicleRegistrationDocumentsExists(Guid ownerId, string fileName)
        {
            return await _niceniceDbContext.VehicleRegistrationDocuments
                .AnyAsync(p => p.OwnerId == ownerId && p.FileName == fileName);
        }
        public async Task<List<VehicleRegistrationDocuments>> GetAllVehicleRegistrationDocuments(Guid ownerId)
        {
            return await _niceniceDbContext.VehicleRegistrationDocuments.Where(x => x.OwnerId == ownerId).ToListAsync();
        }
        public async Task<VehicleRegistrationDocuments> GetVehicleRegistrationDocuments(Guid id)
        {
            return await _niceniceDbContext.VehicleRegistrationDocuments.FindAsync(id);
        }
        public async Task DeleteVehicleRegistrationDocuments(Guid id)
        {
            var file = await _niceniceDbContext.VehicleRegistrationDocuments.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.VehicleRegistrationDocuments.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddVehicleLicenseDiskAndOperatingCard(VehicleLicenseDiskAndOperatingCard model)
        {
            await _niceniceDbContext.VehicleLicenseDiskAndOperatingCard.AddAsync(model);
        }
        public async Task<bool> VehicleLicenseDiskAndOperatingCardExists(Guid ownerId, string fileName)
        {
            return await _niceniceDbContext.VehicleLicenseDiskAndOperatingCard
                .AnyAsync(p => p.OwnerId == ownerId && p.FileName == fileName);
        }
        public async Task<List<VehicleLicenseDiskAndOperatingCard>> GetAllVehicleLicenseDiskAndOperatingCard(Guid ownerId)
        {
            return await _niceniceDbContext.VehicleLicenseDiskAndOperatingCard.Where(x => x.OwnerId == ownerId).ToListAsync();
        }
        public async Task<VehicleLicenseDiskAndOperatingCard> GetVehicleLicenseDiskAndOperatingCard(Guid id)
        {
            return await _niceniceDbContext.VehicleLicenseDiskAndOperatingCard.FindAsync(id);
        }
        public async Task DeleteVehicleLicenseDiskAndOperatingCard(Guid id)
        {
            var file = await _niceniceDbContext.VehicleLicenseDiskAndOperatingCard.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.VehicleLicenseDiskAndOperatingCard.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddProofOfInsurance(ProofOfInsurance model)
        {
            await _niceniceDbContext.ProofOfInsurance.AddAsync(model);
        }
        public async Task<bool> ProofOfInsuranceExists(Guid ownerId, string fileName)
        {
            return await _niceniceDbContext.ProofOfInsurance
                .AnyAsync(p => p.OwnerId == ownerId && p.FileName == fileName);
        }
        public async Task<List<ProofOfInsurance>> GetAllProofOfInsurance(Guid ownerId)
        {
            return await _niceniceDbContext.ProofOfInsurance.Where(x => x.OwnerId == ownerId).ToListAsync();
        }
        public async Task<ProofOfInsurance> GetProofOfInsurance(Guid id)
        {
            return await _niceniceDbContext.ProofOfInsurance.FindAsync(id);
        }
        public async Task DeleteProofOfInsurance(Guid id)
        {
            var file = await _niceniceDbContext.ProofOfInsurance.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.ProofOfInsurance.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddCarPictures(CarPictures model)
        {
            await _niceniceDbContext.CarPictures.AddAsync(model);
        }
        public async Task<bool> CarPicturesExists(Guid? ownerId, string fileName)
        {
            return await _niceniceDbContext.CarPictures
                .AnyAsync(p => p.Id == ownerId && p.FileName == fileName);
        }
        public async Task<List<CarPictures>> GetAllCarPictures(Guid id)
        {
            return await _niceniceDbContext.CarPictures.Where(x => x.Id == id).ToListAsync();
        }
        public async Task<CarPictures> GetCarPictures(Guid id)
        {
            return await _niceniceDbContext.CarPictures.FindAsync(id);
        }
        public async Task DeleteCarPictures(Guid id)
        {
            var file = await _niceniceDbContext.CarPictures.FirstOrDefaultAsync(x => x.CarId == id);
            if (file != null)
            {
                _niceniceDbContext.CarPictures.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }

        public async Task AddCar(Cars model, CancellationToken cancellationToken = default)
        {
            //var transaction = await _niceniceDbContext.Database.BeginTransactionAsync(cancellationToken);
            //try
            //{
            //CarPictures carPicturesEntry = new CarPictures();
            //carPicturesEntry.CarId = Guid.NewGuid();
            //carPicturesEntry.Id = model.Id;

            //if (carPictures != null && carPictures.Length > 0)
            //{
            //    foreach (var file in carPictures)
            //    {
            //        using (var memoryStream = new MemoryStream())
            //        {
            //            await file.CopyToAsync(memoryStream);
            //            carPicturesEntry = new CarPictures
            //            {
            //                //Id = Guid.NewGuid(),
            //                //OwnerId = ownerId,
            //                FileData = memoryStream.ToArray(),
            //                FileName = file.FileName,
            //                FileType = file.ContentType,
            //                Created = DateTime.Now
            //            };
            //        }
            //        await _niceniceDbContext.CarPictures.AddAsync(carPicturesEntry, cancellationToken);

            //    }

            //}
            await _niceniceDbContext.Cars.AddAsync(model, cancellationToken);
            //    await transaction.CommitAsync(cancellationToken);
            //}
            //catch
            //{
            //    await transaction.RollbackAsync(cancellationToken);
            //    throw;
            //}
        }
        public async Task<bool> CarExists(string registrationNumber, CancellationToken cancellationToken = default)
        {
            return await _niceniceDbContext.Cars
                .AnyAsync(p => p.RegistrationNumber == registrationNumber, cancellationToken);
        }
        public async Task UpdateCar(Cars model, CancellationToken cancellationToken = default)
        {
            var car = await _niceniceDbContext.Cars.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

            if (car != null)
            {
                if (model.OwnerId.HasValue)
                    car.OwnerId = model.OwnerId;
                if (model.ColorId.HasValue)
                    car.ColorId = model.ColorId;
                if (model.CarTypeId.HasValue)
                    car.CarTypeId = model.CarTypeId;
                if (!string.IsNullOrEmpty(model.RegistrationNumber))
                    car.RegistrationNumber = model.RegistrationNumber;
                if (!string.IsNullOrEmpty(model.Name))
                    car.Name = model.Name;
                if (model.RegistrationExpirydate.HasValue)
                    car.RegistrationExpirydate = model.RegistrationExpirydate;
                if (!string.IsNullOrEmpty(model.Mileage))
                    car.Mileage = model.Mileage;
                if (model.PlatformId.HasValue)
                    car.PlatformId = model.PlatformId;
                if (!string.IsNullOrEmpty(model.WeeklyRental))
                    car.WeeklyRental = model.WeeklyRental;
                if (!string.IsNullOrEmpty(model.Make))
                    car.Make = model.Make;
                if (model.Year.HasValue)
                    car.Year = model.Year;
                if (!string.IsNullOrEmpty(model.DriverAssignee))
                    car.DriverAssignee = model.DriverAssignee;
                if (model.DateAssigned.HasValue)
                    car.DateAssigned = model.DateAssigned;

                await _niceniceDbContext.SaveChangesAsync(cancellationToken);


            }
        }
        public async Task DeleteCar(Guid id, CancellationToken cancellationToken = default)
        {
            var car = await _niceniceDbContext.Cars.FindAsync(id, cancellationToken);
            if (car != null)
            {
                _niceniceDbContext.Cars.Remove(car);
            }
            else
            {
                throw new InvalidOperationException("No car found");
            }
        }
        public async Task<List<Cars>> GetCarsListByOwnerId(Guid ownerId)
        {
            var cars = await _niceniceDbContext.Cars
                .Where(c => c.OwnerId == ownerId)
                .OrderByDescending(c => c.Created)
                .ToListAsync();

            foreach (var car in cars)
            {
                // 🔍 Get latest driver assigned from Payments table
                var payment = await _niceniceDbContext.Payments
                    .Where(p => p.CarId == car.Id)
                    .OrderByDescending(p => p.PaidAt)
                    .FirstOrDefaultAsync();

                if (payment != null)
                {
                    var driver = await _niceniceDbContext.Drivers
                        .FirstOrDefaultAsync(d => d.Id == payment.DriverId);

                    if (driver != null)
                    {
                        car.DriverAssignee = $"{driver.FirstName} {driver.Surname}";
                    }
                }
            }

            return cars;
        }
        public async Task<Cars> GetCarById(Guid id)
        {
            return await _niceniceDbContext.Cars
                .FromSqlRaw("SELECT * FROM [Owners].[Cars] WHERE Id = {0}", id)
                .Include(c => c.CarType)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<CarDto>> GetAllVehicles()
        {
            var cars = await _niceniceDbContext.Cars
                .FromSqlRaw("SELECT * FROM [Owners].[Cars]")
                .Include(c => c.CarType)
                .ToListAsync();

            var carDtos = new List<CarDto>();

            foreach (var car in cars)
            {
                var owner = await _niceniceDbContext.Owners
                    .FirstOrDefaultAsync(o => o.Id == car.OwnerId);

                var image = await _niceniceDbContext.CarPictures
                    .Where(p => p.CarId == car.Id)
                    .Select(p => new { p.Id })
                    .FirstOrDefaultAsync();

                var pricing = await _niceniceDbContext.VehiclePricings
                    .FirstOrDefaultAsync(p =>
                        p.VehicleType.Replace(" ", "").ToLower() ==
                        car.CarType.Name.Replace(" ", "").ToLower());

                carDtos.Add(new CarDto
                {
                    Id = car.Id.Value,
                    Make = car.Make,
                    Model = car.Model,
                    RegistrationNumber = car.RegistrationNumber,
                    Year = car.Year.GetValueOrDefault(),
                    WeeklyRental = pricing != null ? (int)pricing.WeeklyRental : 0,
                    Mileage = car.Mileage ?? "N/A",
                    OwnerName = owner != null ? $"{owner.FirstName} {owner.Surname}" : "Owner",
                    Location = owner?.Surbub ?? "Unknown Location",
                    ImageUrl = image != null ? $"/api/Owners/downLoadCarPictures/{image.Id}" : null,
                    Created = car.Created,
                    CarTypeId = car.CarTypeId ?? 0,
                    Pricing = pricing != null
                        ? new PricingDto
                        {
                            AdminFee = (int)pricing.AdminFee,
                            Deposit = (int)pricing.Deposit,
                            MinContractMonths = pricing.MinContractMonths,
                            MaxContractMonths = pricing.MaxContractMonths
                        }
                    : null
                });
            }

            return carDtos;
        }
        public async Task<Guid?> GetOwnerIdByUserId(Guid userId)
        {
            var user = await _niceniceDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var owner = await _niceniceDbContext.Owners.FirstOrDefaultAsync(o => o.Email == user.Email);
            return owner?.Id;
        }
        public async Task<List<OwnerEarningDto>> GetWeeklyEarnings(Guid ownerId)
        {
            return await _niceniceDbContext.Payments
            .Include(p => p.Car)
                .ThenInclude(c => c.CarType)
            .Include(p => p.Car)
                .ThenInclude(c => c.CarPictures)
            .Include(p => p.Driver)
            .Where(p => p.Car != null && p.Car.OwnerId == ownerId)
            .GroupBy(p => p.CarId)
            .Select(g => new OwnerEarningDto
            {
                CarId = g.Key,
                CarName = g.First().Car.Make + " " + g.First().Car.Model,
                TotalEarnings = g.Sum(p => p.AmountPaid),
                Commission = g.Sum(p => p.AmountPaid * 0.05m),
                NetIncome = g.Sum(p => p.AmountPaid * 0.95m),
                PaymentCount = g.Count(),
                LastPaymentDate = g.Max(p => p.PaidAt),
                LatestWeeklyRentalAmount = g.OrderByDescending(p => p.PaidAt).Select(p => p.WeeklyRentalAmount).FirstOrDefault(),
                DriverName = g.First().Driver.FirstName,
                DriverSurname = g.First().Driver.Surname
            }).ToListAsync();
        }
        public IQueryable<Cars> GetQueryableVehicles()
        {
            return _niceniceDbContext.Cars.AsQueryable();
        }
        public async Task<BankingDetail?> GetBankingDetailByOwnerId(Guid ownerId)
        {
            return await _niceniceDbContext.BankingDetails.FirstOrDefaultAsync(b => b.OwnerId == ownerId);
        }

        public async Task AddBankingDetail(BankingDetail detail)
        {
            await _niceniceDbContext.BankingDetails.AddAsync(detail);
        }
        public async Task<IEnumerable<object>> GetAllBankingDetails()
        {
            return await _niceniceDbContext.BankingDetails
                .Include(b => b.Owner)
                .Select(b => new {
                    b.Id,
                    OwnerName = b.Owner.FirstName + " " + b.Owner.Surname,
                    b.BankName,
                    b.AccountNumber,
                    b.AccountType,
                    b.BranchCode
                }).ToListAsync();
        }
    }
}
