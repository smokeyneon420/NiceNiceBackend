using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Data.UnitOfWorks;
using nicenice.Server.Models;
using nicenice.Server.Services;
using nicenice.Server.Models.DTOs;
using Microsoft.EntityFrameworkCore;


namespace nicenice.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly UserManager<NiceUser> _userManager;
        private readonly IRepoUnitOfWorks _repoUnitOfWorks;
        private readonly IIdentityServices _identityServices;
        private readonly CommonServices _commonServices;
        public OwnersController(UserManager<NiceUser> userManager, IRepoUnitOfWorks repoUnitOfWorks, IIdentityServices identityServices, CommonServices commonServices)
        {
            _userManager = userManager;
            _repoUnitOfWorks = repoUnitOfWorks;
            _identityServices = identityServices;
            _commonServices = commonServices;
        }

        [HttpGet("ownerProfile")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetOwnerProfile()
        {
            var user = _identityServices.CurrentUser;
            var currentUser = await _repoUnitOfWorks.ownerRepo.GetFirstOwnerProfile(user.Email);
            if (currentUser == null)
            {
                return Ok(new { currentUser = new { FirstName = user.FirstName, Surname = user.LastName, Email = user.Email }, ownerExist = false });
            }
            else
            {
                var ownerProfile = new
                {


                    currentUser.Id,
                    currentUser.InsuranceClaim,
                    currentUser.NumberOfClaims,
                    currentUser.Created,
                    currentUser.Updated,
                    currentUser.User,
                    currentUser.FirstName,
                    currentUser.Surname,
                    currentUser.Email,
                    currentUser.Contact,
                    currentUser.Telephone,
                    currentUser.IdNumber,
                    currentUser.IsVerified,
                    currentUser.Gender,

                    currentUser.BuildingNumber,
                    currentUser.ProvinceId,
                    currentUser.Surbub,
                    currentUser.TownId,

                    //public string Town { get; set; }
                    //public string Province { get; set; }
                    currentUser.Street,

                    //// Convert byte arrays back to Base64 strings
                    //IdentityDocument = currentUser.IdentityDocument != null ? Convert.ToBase64String(currentUser.IdentityDocument) : null,
                    //VehicleRegistrationDocuments = currentUser.VehicleRegistrationDocuments != null ? Convert.ToBase64String(currentUser.VehicleRegistrationDocuments) : null,
                    //VehicleLicenseDiskAndOperatingCard = currentUser.VehicleLicenseDiskAndOperatingCard != null ? Convert.ToBase64String(currentUser.VehicleLicenseDiskAndOperatingCard) : null,
                    //CarPicture = currentUser.CarPicture != null ? Convert.ToBase64String(currentUser.CarPicture) : null,
                    //ProofOfInsurance = currentUser.ProofOfInsurance != null ? Convert.ToBase64String(currentUser.ProofOfInsurance) : null,
                };
                return Ok(new { currentUser = ownerProfile, ownerExist = true });
            }
        }

        [HttpPost("addUpdateOwnerProfile")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> AddUpdateOwnerProfile([FromBody] Owner model)
        {

            var user = _identityServices.CurrentUser;
            var currentUser = await _repoUnitOfWorks.ownerRepo.GetFirstOwnerProfile(user.Email);
            //model.IdentityDocument = await _commonServices.ConvertFilesToByteArray(identityDocument);
            //model.VehicleRegistrationDocuments = await _commonServices.ConvertFilesToByteArray(vehicleRegistrationDocuments);
            //model.VehicleLicenseDiskAndOperatingCard = await _commonServices.ConvertFilesToByteArray(vehicleLicenseDiskAndOperatingCard);
            //model.CarPicture = await _commonServices.ConvertFilesToByteArray(carPicture);
            //model.CarPicture = await _commonServices.ConvertFilesToByteArray(carPicture);

            if (currentUser == null)
            {
                model.Id = Guid.NewGuid();
                model.Created = DateTime.Now;
                model.Updated = DateTime.Now;
                model.User = model.Email;
                model.IsVerified = false;
                await _repoUnitOfWorks.ownerRepo.AddOwnerProfile(model);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok(new { message = "profile added successfully", model });
            }
            else
            {
                var userUpdate = await _userManager.FindByEmailAsync(model.Email);
                if (userUpdate == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Update user properties
                userUpdate.FirstName = model.FirstName;
                userUpdate.LastName = model.Surname;
                userUpdate.UserName = model.Email;
                userUpdate.Email = model.Email;
                // You can also update other properties if needed

                // Update the user in the database
                var updateResult = await _userManager.UpdateAsync(userUpdate);
                if (!updateResult.Succeeded)
                {
                    // Handle errors from the update operation
                    return BadRequest(updateResult.Errors);
                }
                model.Updated = DateTime.Now;
                model.Updated = DateTime.Now;
                model.User = model.Email;
                //var userUpdate = new NiceUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.Surname, };
                //var results = await _userManager.UpdateAsync(userUpdate);
                await _repoUnitOfWorks.ownerRepo.UpdateOwnerProfile(model);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok(new
                {
                    message = "profile updated successfully",
                    model
                });
            }
        }
        [HttpGet("ownersCount")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetOwnersCount()
        {
            int ownersCount = await _repoUnitOfWorks.ownerRepo.GetOwnerCount();
            return Ok(new { ownersCount });
        }

        [HttpPost("addOwnerFiles")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> AddOwnerFiles([FromForm] Guid ownerId, [FromForm] IFormFile? ownerIdentityDocuments, [FromForm] IFormFile? vehicleRegistrationDocuments, [FromForm] IFormFile? vehicleLicenseDiskAndOperatingCard
            , [FromForm] IFormFile? proofOfInsurance, [FromForm] IFormFile? carPictures)
        {
            OwnerIdentityDocuments ownerIdentityDocumentsEntry = null;
            VehicleRegistrationDocuments vehicleRegistrationDocumentsEntry = null;
            VehicleLicenseDiskAndOperatingCard vehicleLicenseDiskAndOperatingCardEntry = null;
            ProofOfInsurance proofOfInsuranceEntry = null;
            CarPictures carPicturesEntry = null;


            if (ownerIdentityDocuments != null && ownerIdentityDocuments.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ownerIdentityDocuments.CopyToAsync(memoryStream);
                    ownerIdentityDocumentsEntry = new OwnerIdentityDocuments
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = ownerId,
                        FileData = memoryStream.ToArray(),
                        FileName = ownerIdentityDocuments.FileName,
                        FileType = ownerIdentityDocuments.ContentType,
                        Created = DateTime.Now
                    };
                }
            }


            if (vehicleRegistrationDocuments != null && vehicleRegistrationDocuments.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await vehicleRegistrationDocuments.CopyToAsync(memoryStream);
                    vehicleRegistrationDocumentsEntry = new VehicleRegistrationDocuments
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = ownerId,
                        FileData = memoryStream.ToArray(),
                        FileName = vehicleRegistrationDocuments.FileName,
                        FileType = vehicleRegistrationDocuments.ContentType,
                        Created = DateTime.Now
                    };
                }
            }


            if (vehicleLicenseDiskAndOperatingCard != null && vehicleLicenseDiskAndOperatingCard.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await vehicleLicenseDiskAndOperatingCard.CopyToAsync(memoryStream);
                    vehicleLicenseDiskAndOperatingCardEntry = new VehicleLicenseDiskAndOperatingCard
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = ownerId,
                        FileData = memoryStream.ToArray(),
                        FileName = vehicleLicenseDiskAndOperatingCard.FileName,
                        FileType = vehicleLicenseDiskAndOperatingCard.ContentType,
                        Created = DateTime.Now
                    };
                }
            }

            if (proofOfInsurance != null && proofOfInsurance.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await proofOfInsurance.CopyToAsync(memoryStream);
                    proofOfInsuranceEntry = new ProofOfInsurance
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = ownerId,
                        FileData = memoryStream.ToArray(),
                        FileName = proofOfInsurance.FileName,
                        FileType = proofOfInsurance.ContentType,
                        Created = DateTime.Now
                    };
                }
            }

            if (carPictures != null && carPictures.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await carPictures.CopyToAsync(memoryStream);
                    carPicturesEntry = new CarPictures
                    {
                        CarId = Guid.NewGuid(),
                        Id = ownerId,
                        FileData = memoryStream.ToArray(),
                        FileName = carPictures.FileName,
                        FileType = carPictures.ContentType,
                        Created = DateTime.Now
                    };
                }
            }
            if (ownerIdentityDocumentsEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.ownerRepo.OwnerIdentityDocumentsExists(ownerIdentityDocumentsEntry.OwnerId, ownerIdentityDocumentsEntry.FileName);
                if (fileExists)
                {
                    return Conflict("Identity document file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.ownerRepo.AddOwnerIdentityDocuments(ownerIdentityDocumentsEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("Identity Documents file uploaded");
            }
            if (vehicleRegistrationDocumentsEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.ownerRepo.VehicleRegistrationDocumentsExists(vehicleRegistrationDocumentsEntry.OwnerId, vehicleRegistrationDocumentsEntry.FileName);
                if (fileExists)
                {
                    return Conflict("Vehicle registration document file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.ownerRepo.AddVehicleRegistrationDocuments(vehicleRegistrationDocumentsEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("vehicle Registration Documents file uploaded");
            }
            if (vehicleLicenseDiskAndOperatingCardEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.ownerRepo.VehicleLicenseDiskAndOperatingCardExists(vehicleLicenseDiskAndOperatingCardEntry.OwnerId, vehicleLicenseDiskAndOperatingCardEntry.FileName);
                if (fileExists)
                {
                    return Conflict("Vehicle license disk and operating card file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.ownerRepo.AddVehicleLicenseDiskAndOperatingCard(vehicleLicenseDiskAndOperatingCardEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("vehicle License Disk And Operating Card file uploaded");
            }
            if (proofOfInsuranceEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.ownerRepo.ProofOfInsuranceExists(proofOfInsuranceEntry.OwnerId, proofOfInsuranceEntry.FileName);
                if (fileExists)
                {
                    return Conflict("Proof of insurance file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.ownerRepo.AddProofOfInsurance(proofOfInsuranceEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("proof Of Insurance file uploaded");
            }
            if (carPicturesEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.ownerRepo.CarPicturesExists(carPicturesEntry.Id, carPicturesEntry.FileName);
                if (fileExists)
                {
                    return Conflict("Car Picture file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.ownerRepo.AddCarPictures(carPicturesEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("car Pictures file uploaded");
            }

            return Ok("No Files Uploaded");
        }

        [HttpGet("getOwnerIdentityDocuments/{ownerId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetIdentityDocuments(Guid ownerId)
        {
            var files = await _repoUnitOfWorks.ownerRepo.GetAllOwnerIdentityDocuments(ownerId);
            //if (files == null || files.Count == 0)
            //{
            //    return NotFound("No photos found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadOwnerIdentityDocument", new { id = x.Id }) // Generate a download link

            }); await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(response);
        }

        [HttpGet("downLoadOwnerIdentityDocument/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DownLoadOwnerIdentityDocument(Guid id)
        {
            var file = await _repoUnitOfWorks.ownerRepo.GetOwnerIdentityDocuments(id);
            if (file == null)
            {
                return NotFound("No file found");
            }
            await _repoUnitOfWorks.SaveChangesAsync();
            return File(file.FileData, file.FileType, file.FileName, true);
        }
        [HttpDelete("deleteOwnerIdentityDocument/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DeleteOwnerIdentityDocument(Guid id)
        {
            await _repoUnitOfWorks.ownerRepo.DeleteOwnerIdentityDocuments(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }

        [HttpGet("getVehicleRegistrationDocuments/{ownerId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetVehicleRegistrationDocumentsEntry(Guid ownerId)
        {
            var files = await _repoUnitOfWorks.ownerRepo.GetAllVehicleRegistrationDocuments(ownerId);
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No photos found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadVehicleRegistrationDocuments", new { id = x.Id }) // Generate a download link

            }); await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(response);
        }

        [HttpGet("downLoadVehicleRegistrationDocuments/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DownLoadVehicleRegistrationDocuments(Guid id)
        {
            var file = await _repoUnitOfWorks.ownerRepo.GetVehicleRegistrationDocuments(id);
            if (file == null)
            {
                return NotFound("No file found");
            }
            await _repoUnitOfWorks.SaveChangesAsync();
            return File(file.FileData, file.FileType, file.FileName, true);
        }
        [HttpDelete("deleteVehicleRegistrationDocuments/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DeleteVehicleRegistrationDocuments(Guid id)
        {
            await _repoUnitOfWorks.ownerRepo.DeleteVehicleRegistrationDocuments(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getVehicleLicenseDiskAndOperatingCard/{ownerId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetVehicleLicenseDiskAndOperatingCard(Guid ownerId)
        {
            var files = await _repoUnitOfWorks.ownerRepo.GetAllVehicleLicenseDiskAndOperatingCard(ownerId);

            //if (files == null || !files.Any())
            //{
            //    return NotFound("No photos found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownloadVehicleLicenseDiskAndOperatingCard", new { id = x.Id }) // Generate a download link

            });
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(response);
        }

        [HttpGet("downLoadVehicleLicenseDiskAndOperatingCard/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DownLoadVehicleLicenseDiskAndOperatingCard(Guid id)
        {
            var file = await _repoUnitOfWorks.ownerRepo.GetVehicleLicenseDiskAndOperatingCard(id);
            if (file == null)
            {
                return NotFound("No file found");
            }
            await _repoUnitOfWorks.SaveChangesAsync();
            return File(file.FileData, file.FileType, file.FileName, true);
        }
        [HttpDelete("deleteVehicleLicenseDiskAndOperatingCard/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DeleteVehicleLicenseDiskAndOperatingCard(Guid id)
        {
            await _repoUnitOfWorks.ownerRepo.DeleteVehicleLicenseDiskAndOperatingCard(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getProofOfInsurance/{ownerId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetProofOfInsurance(Guid ownerId)
        {
            var files = await _repoUnitOfWorks.ownerRepo.GetAllProofOfInsurance(ownerId);
            //await _repoUnitOfWorks.SaveChangesAsync();
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No photos found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownloadProofOfInsurance", new { id = x.Id }) // Generate a download link

            }); await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(response);
        }

        [HttpGet("downLoadProofOfInsurance/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DownLoadProofOfInsurance(Guid id)
        {
            var file = await _repoUnitOfWorks.ownerRepo.GetProofOfInsurance(id);
            if (file == null)
            {
                return NotFound("No file found");
            }
            await _repoUnitOfWorks.SaveChangesAsync();
            return File(file.FileData, file.FileType, file.FileName, true);
        }
        [HttpDelete("deleteProofOfInsurance/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DeleteProofOfInsurance(Guid id)
        {
            await _repoUnitOfWorks.ownerRepo.DeleteProofOfInsurance(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getCarPictures/{Id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetAllCarPictures(Guid id, CancellationToken cancellationToken = default)
        {
            var files = await _repoUnitOfWorks.ownerRepo.GetAllCarPictures(id);
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No photos found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.CarId,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownloadCarPictures", new { id = x.Id }) // Generate a download link

            }); await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(response);
        }

        [HttpGet("downLoadCarPictures/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DownLoadCarPictures(Guid id, CancellationToken cancellationToken = default)
        {
            var file = await _repoUnitOfWorks.ownerRepo.GetCarPictures(id);
            if (file == null)
            {
                return NotFound("No file found");
            }
            await _repoUnitOfWorks.SaveChangesAsync();
            return File(file.FileData, file.FileType, file.FileName, true);
        }
        [HttpDelete("deleteCarPictures/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DeleteCarPictures(Guid id, CancellationToken cancellationToken = default)
        {
            await _repoUnitOfWorks.ownerRepo.DeleteCarPictures(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }

        [HttpPost("addCar")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> AddCar([FromBody] Cars model, CancellationToken cancellationToken = default)
        {
            var userId = _identityServices.GetUserId();

            if (userId == null)
                return BadRequest("User ID not found.");

            var ownerId = await _repoUnitOfWorks.ownerRepo.GetOwnerIdByUserId(userId.Value);

            if (ownerId == null)
                return BadRequest("Owner profile not found for this user.");

            model.OwnerId = ownerId.Value;
            model.Created = DateTime.Now;
            model.Updated = DateTime.Now;

            bool carExist = await _repoUnitOfWorks.ownerRepo.CarExists(model.RegistrationNumber, cancellationToken);

            if (!carExist && model.Id == null)
            {
                model.Id = Guid.NewGuid();
                await _repoUnitOfWorks.ownerRepo.AddCar(model, cancellationToken);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok(new { success = true, message = $"The car added {model.RegistrationNumber} successfully" });
            }
            else if (model.Id != null)
            {
                await _repoUnitOfWorks.ownerRepo.UpdateCar(model, cancellationToken);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok(new { success = true, message = $"The car updated {model.RegistrationNumber} successfully" });
            }
            else
            {
                return Ok(new { success = false, message = $"The car with this registration {model.RegistrationNumber} already exists" });
            }
        }


        [HttpPut("updateCar")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> UpdateCar([FromBody] Cars model)
        {
            await _repoUnitOfWorks.ownerRepo.UpdateCar(model);
            return Ok("Car Updated");
        }
        [HttpGet("getCarsListByOwnerId/{ownerId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetCarsListByOwnerId(Guid ownerId)
        {
            var rowsAffected = await _repoUnitOfWorks.ownerRepo.GetCarsListByOwnerId(ownerId);
            return Ok(rowsAffected);
        }
        [AllowAnonymous]
        [HttpGet("getCarById/{id}")]
        public async Task<IActionResult> GetCarById(Guid id)
        {
            var rowsAffected = await _repoUnitOfWorks.ownerRepo.GetCarById(id);
            return Ok(rowsAffected);
        }
        [HttpDelete("deleteCar/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            await _repoUnitOfWorks.ownerRepo.DeleteCar(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("deleted");
        }
        [HttpGet("getAllVehicles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _repoUnitOfWorks.ownerRepo.GetAllVehicles();
            return Ok(vehicles);
        }
        [HttpGet("getOwnerIdByUserId/{userId}")]
        public async Task<IActionResult> GetOwnerIdByUserId(Guid userId)
        {
            var ownerId = await _repoUnitOfWorks.ownerRepo.GetOwnerIdByUserId(userId);
            if (ownerId == null)
                return NotFound("Owner profile not found");

            return Ok(ownerId);
        }
        [HttpGet("getOwnerEarnings/{ownerId}")]
        public async Task<IActionResult> GetOwnerEarnings(Guid ownerId)
        {
            var earnings = await _repoUnitOfWorks.ownerRepo.GetWeeklyEarnings(ownerId);
            return Ok(earnings);
        }
        [HttpGet("GetAllCars")]
        public async Task<IActionResult> GetAllCars(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var query = _repoUnitOfWorks.ownerRepo.GetQueryableVehicles();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c =>
                    c.Make.Contains(searchTerm) || 
                    c.Model.Contains(searchTerm) || 
                    c.RegistrationNumber.Contains(searchTerm));
            }

            var totalRecords = await query.CountAsync();
            var cars = await query
                .OrderByDescending(c => c.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var carDtos = cars.Select(car => new CarDto
            {
                Id = car.Id.Value,
                Make = car.Make,
                Model = car.Model,
                RegistrationNumber = car.RegistrationNumber,
                Year = car.Year.GetValueOrDefault(),
                WeeklyRental = int.TryParse(car.WeeklyRental, out var rental) ? rental : 0,
                OwnerName = "Owner",
                Location = "Unknown",
                Created = car.Created
            });

            return Ok(new { totalRecords, data = carDtos });
        }
        [HttpPost("uploadBankDetails")]
        public async Task<IActionResult> UploadBankDetails([FromForm] BankDetailsDto dto)
        {
            if (dto == null || dto.OwnerId == Guid.Empty)
                return BadRequest("Invalid data.");

            var existing = await _repoUnitOfWorks.ownerRepo.GetBankingDetailByOwnerId(dto.OwnerId);
            if (existing == null)
            {
                existing = new BankingDetail
                {
                    Id = Guid.NewGuid(),
                    OwnerId = dto.OwnerId,
                    CreatedAt = DateTime.UtcNow
                };
                await _repoUnitOfWorks.ownerRepo.AddBankingDetail(existing);
            }

            existing.AccountHolderName = dto.AccountHolderName;
            existing.BankName = dto.BankName;
            existing.AccountNumber = dto.AccountNumber;
            existing.AccountType = dto.AccountType;
            existing.BranchCode = dto.BranchCode;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(new { message = "Banking details saved successfully." });
        }
        [HttpGet("getBankingDetails/{ownerId}")]
        public async Task<IActionResult> GetBankingDetails(Guid ownerId)
        {
            var bankingDetail = await _repoUnitOfWorks.ownerRepo.GetBankingDetailByOwnerId(ownerId);

            if (bankingDetail == null)
                return NotFound("No banking details found.");

            return Ok(new
            {
                bankingDetail.AccountHolderName,
                bankingDetail.BankName,
                bankingDetail.AccountNumber,
                bankingDetail.BranchCode,
                bankingDetail.AccountType,
            });
        }
    }
}