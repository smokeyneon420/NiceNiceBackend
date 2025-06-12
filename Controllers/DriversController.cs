using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Data.UnitOfWorks;
using nicenice.Server.Models;
using nicenice.Server.Services;

namespace nicenice.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireLoggedIn")]

    public class DriversController : ControllerBase
    {
        private readonly IRepoUnitOfWorks _repoUnitOfWorks;
        private readonly IIdentityServices _identityService;
        private readonly UserManager<NiceUser> _userManager;
        private readonly CommonServices _commonServices;
        public DriversController(IRepoUnitOfWorks repoUnitOfWorks, IIdentityServices identityService, UserManager<NiceUser> userManager, CommonServices commonServices)
        {
            _repoUnitOfWorks = repoUnitOfWorks;
            _identityService = identityService;
            _userManager = userManager;
            _commonServices = commonServices;
        }

        [HttpGet("driverProfile")]
        public async Task<IActionResult> GetDriverProfile()
        {
            try
            {
                var user = _identityService.CurrentUser;
                var currentUser = await _repoUnitOfWorks.driverRepo.GetFirstDriverProfile(user.Email);

                if (currentUser == null)
                {
                    return Ok(new { currentUser = new { FirstName = user.FirstName, Surname = user.LastName, Email = user.Email }, driverExist = false });
                }
                else
                {
                    var driverProfile = new
                    {
                        currentUser.Id,
                        currentUser.FirstName,
                        currentUser.Surname,
                        currentUser.Email,
                        currentUser.Contact,
                        currentUser.Telephone,
                        currentUser.YearsExperience,
                        currentUser.CriminalRecord,
                        currentUser.InsuranceClaim,
                        currentUser.NumberOfClaims,
                        currentUser.IsVerified,
                        currentUser.Gender,
                        currentUser.BuildingNumber,
                        currentUser.ProvinceId,
                        currentUser.Surbub,
                        currentUser.TownId,
                        currentUser.Street,
                    };

                    return Ok(new { currentUser = driverProfile, driverExist = true });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occured", ex.Message });
            }
        }

        [HttpPost("addUpdateDriverProfile")]
        public async Task<IActionResult> AddUpdateDriverProfile([FromBody] Drivers model)
        {
            var user = _identityService.CurrentUser;
            var currentUser = await _repoUnitOfWorks.driverRepo.GetFirstDriverProfile(user.Email);

            if (currentUser == null)
            {
                model.Id = Guid.NewGuid();
                model.Created = DateTime.Now;
                model.Updated = DateTime.Now;
                model.User = model.Email;
                model.IsVerified = false;
                await _repoUnitOfWorks.driverRepo.AddDriverProfile(model);
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

                userUpdate.FirstName = model.FirstName;
                userUpdate.LastName = model.Surname;
                userUpdate.UserName = model.Email;
                userUpdate.Email = model.Email;
                userUpdate.PhoneNumber = model.Contact;

                var updateResult = await _userManager.UpdateAsync(userUpdate);
                if (!updateResult.Succeeded)
                {
                    return BadRequest(updateResult.Errors);
                }

                model.Updated = DateTime.Now;
                model.User = model.Email;
                await _repoUnitOfWorks.driverRepo.UpdateDriverProfile(model);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok(new { message = "profile updated successfully", model });
            }
        }

        [HttpGet]
        [Route("driversCount")]
        public async Task<IActionResult> GetDriverCount()
        {
            try
            {
                int driverCount = await _repoUnitOfWorks.driverRepo.GetDriversCount();
                await _repoUnitOfWorks.SaveChangesAsync();
                Console.WriteLine(driverCount);
                return Ok(new { driverCount });


            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching driver count", error = ex.Message });

            }
        }
        //}
        [HttpPost("addDriversFiles")]
        public async Task<IActionResult> AddDriverFiles([FromForm] Guid driverId, [FromForm] IFormFile? profilePhotos, [FromForm] IFormFile? identityDocuments, [FromForm] IFormFile? licenseCopies
            , [FromForm] IFormFile? fingerPrintReports, [FromForm] IFormFile? currentPlatformScreenshots, [FromForm] IFormFile? selfiePics, [FromForm] IFormFile? passports, [FromForm] IFormFile? permitOrAsylumLetters
             , [FromForm] IFormFile? bankConfirmationLetters, [FromForm] IFormFile? proofOfResidenceOrBankStatements)
        {

            var user = _identityService.CurrentUser;
            Profilephotos profilephotosEntry = null;
            IdentityDocuments identityDocumentsEntry = null;
            LicenseCopies licenseCopiesEntry = null;
            FingerPrintReports fingerPrintsReportEntry = null;
            CurrentPlatformScreenshots currentPlatformScreenshotsEntry = null;
            SelfiePics selfiePicsEntry = null;
            Passports passportEntry = null;
            PermitOrAsylumLetters permitOrAsylumLettersEntry = null;
            BankConfirmationLetters bankConfirmationEntry = null;
            ProofOfResidenceOrBankStatements proofOfResidenceOrBankStatementsEntry = null;


            if (profilePhotos != null && profilePhotos.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePhotos.CopyToAsync(memoryStream);
                    profilephotosEntry = new Profilephotos
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = profilePhotos.FileName,
                        FileType = profilePhotos.ContentType,
                        Created = DateTime.Now
                    };
                }
            }

            if (identityDocuments != null && identityDocuments.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await identityDocuments.CopyToAsync(memoryStream);
                    identityDocumentsEntry = new IdentityDocuments
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = identityDocuments.FileName,
                        FileType = identityDocuments.ContentType,
                        Created = DateTime.Now
                    };
                }
            }

            if (licenseCopies != null && licenseCopies.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await licenseCopies.CopyToAsync(memoryStream);
                    licenseCopiesEntry = new LicenseCopies
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = licenseCopies.FileName,
                        FileType = licenseCopies.ContentType,
                        Created = DateTime.Now
                    };
                }
            }

            if (fingerPrintReports != null && fingerPrintReports.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fingerPrintReports.CopyToAsync(memoryStream);
                    fingerPrintsReportEntry = new FingerPrintReports
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = fingerPrintReports.FileName,
                        FileType = fingerPrintReports.ContentType,
                        Created = DateTime.Now
                    };
                }
            }

            if (currentPlatformScreenshots != null && currentPlatformScreenshots.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await currentPlatformScreenshots.CopyToAsync(memoryStream);
                    currentPlatformScreenshotsEntry = new CurrentPlatformScreenshots
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = currentPlatformScreenshots.FileName,
                        FileType = currentPlatformScreenshots.ContentType,
                        Created = DateTime.Now
                    };
                }
            }

            if (selfiePics != null && selfiePics.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await selfiePics.CopyToAsync(memoryStream);
                    selfiePicsEntry = new SelfiePics
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = selfiePics.FileName,
                        FileType = selfiePics.ContentType,
                        Created = DateTime.Now
                    };
                }
            }
            if (passports != null && passports.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await passports.CopyToAsync(memoryStream);
                    passportEntry = new Passports
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = passports.FileName,
                        FileType = passports.ContentType,
                        Created = DateTime.Now
                    };
                }
            }
            if (permitOrAsylumLetters != null && permitOrAsylumLetters.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await permitOrAsylumLetters.CopyToAsync(memoryStream);
                    permitOrAsylumLettersEntry = new PermitOrAsylumLetters
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = permitOrAsylumLetters.FileName,
                        FileType = permitOrAsylumLetters.ContentType,
                        Created = DateTime.Now
                    };
                }
            }
            if (bankConfirmationLetters != null && bankConfirmationLetters.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await bankConfirmationLetters.CopyToAsync(memoryStream);
                    bankConfirmationEntry = new BankConfirmationLetters
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = bankConfirmationLetters.FileName,
                        FileType = bankConfirmationLetters.ContentType,
                        Created = DateTime.Now
                    };
                }
            }

            if (proofOfResidenceOrBankStatements != null && proofOfResidenceOrBankStatements.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await proofOfResidenceOrBankStatements.CopyToAsync(memoryStream);
                    proofOfResidenceOrBankStatementsEntry = new ProofOfResidenceOrBankStatements
                    {
                        Id = Guid.NewGuid(),
                        DriverId = driverId,
                        FileData = memoryStream.ToArray(),
                        FileName = proofOfResidenceOrBankStatements.FileName,
                        FileType = proofOfResidenceOrBankStatements.ContentType,
                        Created = DateTime.Now
                    };
                }
            }



            if (profilephotosEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.ProfilePhotoExists(profilephotosEntry.DriverId, profilephotosEntry.FileName);
                if (fileExists)
                {
                    return Conflict("Profile photo file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddProfilePhotos(profilephotosEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("Profile photo file uploaded");
            }
            if (identityDocumentsEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.IdentityDocumentsExists(identityDocumentsEntry.DriverId, identityDocumentsEntry.FileName);
                if (fileExists)
                {
                    return Conflict("Identity document file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddIdentityDocuments(identityDocumentsEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("identity Document file uploaded");
            }
            if (licenseCopiesEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.LicenseCopiesExists(licenseCopiesEntry.DriverId, licenseCopiesEntry.FileName);
                if (fileExists)
                {
                    return Conflict("license copy file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddLicenseCopies(licenseCopiesEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("license Copy file uploaded");
            }
            if (fingerPrintsReportEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.FingerPrintReportsExists(fingerPrintsReportEntry.DriverId, fingerPrintsReportEntry.FileName);
                if (fileExists)
                {
                    return Conflict("finger print report file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddFingerPrintReports(fingerPrintsReportEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("finger Prints Report file uploaded");
            }
            if (currentPlatformScreenshotsEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.CurrentPlatformScreenshotsExists(currentPlatformScreenshotsEntry.DriverId, currentPlatformScreenshotsEntry.FileName);
                if (fileExists)
                {
                    return Conflict("current platform screenshot file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddCurrentPlatformScreenshots(currentPlatformScreenshotsEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("current Platform Screenshots file uploaded");
            }
            if (selfiePicsEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.SelfiePicsExists(selfiePicsEntry.DriverId, selfiePicsEntry.FileName);
                if (fileExists)
                {
                    return Conflict("selfie pic file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddSelfiePics(selfiePicsEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("selfie Pic file uploaded");
            }
            if (passportEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.PassportsExists(passportEntry.DriverId, passportEntry.FileName);
                if (fileExists)
                {
                    return Conflict("passport file already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddPassports(passportEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("passport file uploaded");
            }
            if (permitOrAsylumLettersEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.PermitOrAsylumLettersExists(permitOrAsylumLettersEntry.DriverId, permitOrAsylumLettersEntry.FileName);
                if (fileExists)
                {
                    return Conflict("permit or asylum letter already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddPermitOrAsylumLetters(permitOrAsylumLettersEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("permit Or Asylum Letters file uploaded");
            }
            if (bankConfirmationEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.BankConfirmationLettersExists(bankConfirmationEntry.DriverId, bankConfirmationEntry.FileName);
                if (fileExists)
                {
                    return Conflict("bank confirmation letter already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddBankConfirmationLetters(bankConfirmationEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("bank Confirmation file uploaded");
            }
            if (proofOfResidenceOrBankStatementsEntry != null)
            {
                bool fileExists = await _repoUnitOfWorks.driverRepo.ProofOfResidenceOrBankStatementsExists(proofOfResidenceOrBankStatementsEntry.DriverId, proofOfResidenceOrBankStatementsEntry.FileName);
                if (fileExists)
                {
                    return Conflict("proof of residence or bank statement already exists."); // 409 Conflict
                }
                await _repoUnitOfWorks.driverRepo.AddProofOfResidenceOrBankStatements(proofOfResidenceOrBankStatementsEntry);
                await _repoUnitOfWorks.SaveChangesAsync();
                return Ok("proof Of Residence Or Bank Statements file uploaded");
            }

            return Ok("No Files Uploaded");
            //ProofOfResidenceOrBankStatements proofOfResidenceOrBankStatementsEntry = null;
        }

        [HttpGet("getProfilePhotos/{driverId}")]
        public async Task<IActionResult> GetProfilePhotos(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllProfilephotos(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            //if(files == null || !files.Any())
            //{
            //    return NotFound("No photos found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownloadPhoto", new { id = x.Id }) // Generate a download link

            });
            return Ok(response);
        }

        [HttpGet("downLoadPhoto/{id}")]
        public async Task<IActionResult> DownLoadPhoto(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetProfilephoto(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName, true);
        }
        [HttpDelete("deletePhoto/{id}")]
        public async Task<IActionResult> DeletePhoto(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeleteProfilePhoto(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getAllIdentityDocuments/{driverId}")]
        public async Task<IActionResult> GetAllIdentityDocuments(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllIdentityDocuments(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No identity document found for this driver");
            //}

            var rowsAffected = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadIdentityDocument", new { id = x.Id }) // Generate a download link

            });
            return Ok(rowsAffected);
        }

        [HttpGet("downLoadIdentityDocument/{id}")]
        public async Task<IActionResult> DownLoadIdentityDocument(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetIdentityDocument(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName);
        }
        [HttpDelete("deleteIdentityDocument/{id}")]
        public async Task<IActionResult> DeleteIdentityDocument(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeleteIdentityDocuments(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getAllLicenseCopies/{driverId}")]
        public async Task<IActionResult> GetAllLicenseCopies(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllLicenseCopies(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No license copies found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadLicenseCopy", new { id = x.Id }) // Generate a download link

            });
            return Ok(response);
        }

        [HttpGet("downLoadLicenseCopy/{id}")]
        public async Task<IActionResult> DownLoadLicenseCopy(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetLicenseCopy(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName);
        }

        [HttpDelete("deleteLicenseCopy/{id}")]
        public async Task<IActionResult> DeleteLicenseCopy(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeleteLicenseCopies(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getAllFingerPrintReports/{driverId}")]
        public async Task<IActionResult> GetAllFingerPrintReports(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllFingerPrintReports(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No finger reports found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadFingerPrintReport", new { id = x.Id }) // Generate a download link

            });
            return Ok(response);
        }

        [HttpGet("downLoadFingerPrintReport/{id}")]
        public async Task<IActionResult> DownLoadFingerPrintReport(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetFingerPrintReport(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName);
        }
        [HttpDelete("deleteFingerPrintReport/{id}")]
        public async Task<IActionResult> DeleteFingerPrintReport(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeleteFingerPrintReports(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }

        [HttpGet("getAllCurrentPlatformScreenshots/{driverId}")]
        public async Task<IActionResult> GetAllCurrentPlatformScreenshots(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllCurrentPlatformScreenshots(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No current screen shots found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadCurrentPlatformScreenshot", new { id = x.Id }) // Generate a download link

            });
            return Ok(response);
        }

        [HttpGet("downLoadCurrentPlatformScreenshot/{id}")]
        public async Task<IActionResult> DownLoadCurrentPlatformScreenshot(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetCurrentPlatformScreenshot(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName);
        }
        [HttpDelete("deleteCurrentPlatformScreenshot/{id}")]
        public async Task<IActionResult> DeleteCurrentPlatformScreenshot(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeleteCurrentPlatformScreenshots(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getAllSelfiePics/{driverId}")]
        public async Task<IActionResult> GetAllSelfiePics(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllSelfiePics(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No selfie pic found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadSelfiePic", new { id = x.Id }) // Generate a download link

            });
            return Ok(response);
        }

        [HttpGet("downLoadSelfiePic/{id}")]
        public async Task<IActionResult> DownLoadSelfiePic(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetSelfiePic(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName);
        }
        [HttpDelete("deleteSelfiePic/{id}")]
        public async Task<IActionResult> DeleteSelfiePic(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeleteSelfiePics(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getAllPassports/{driverId}")]
        public async Task<IActionResult> GetAllPassports(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllPassports(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No passport found for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadPassport", new { id = x.Id }) // Generate a download link

            });
            return Ok(response);
        }

        [HttpGet("downLoadPassport/{id}")]
        public async Task<IActionResult> DownLoadPassport(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetPassport(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName);
        }
        [HttpDelete("deletePassport/{id}")]
        public async Task<IActionResult> DeletePassport(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeletePassports(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }

        [HttpGet("getAllPermitOrAsylumLetters/{driverId}")]
        public async Task<IActionResult> GetAllPermitOrAsylumLetters(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllPermitOrAsylumLetters(driverId);

            //if (files == null || !files.Any())
            //{
            //    return NotFound("No permit or asylum letter for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadPermitOrAsylumLetter", new { id = x.Id }) // Generate a download link

            });
            return Ok(response);
        }

        [HttpGet("downLoadPermitOrAsylumLetter/{id}")]
        public async Task<IActionResult> DownLoadPermitOrAsylumLetter(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetPermitOrAsylumLetter(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName);
        }
        [HttpDelete("deletePermitOrAsylumLetter/{id}")]
        public async Task<IActionResult> deletePermitOrAsylumLetter(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeletePermitOrAsylumLetters(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getAllBankConfirmationLetters/{driverId}")]
        public async Task<IActionResult> GetAllBankConfirmationLetters(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllBankConfirmationLetters(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No pbank confirmation  letter for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadBankConfirmationLetter", new { id = x.Id }) // Generate a download link

            });
            return Ok(response);
        }

        [HttpGet("downLoadBankConfirmationLetter/{id}")]
        public async Task<IActionResult> DownLoadBankConfirmationLetter(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetBankConfirmationLetter(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName);
        }
        [HttpDelete("deleteBankConfirmationLetter/{id}")]
        public async Task<IActionResult> DeleteBankConfirmationLetter(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeleteBankConfirmationLetters(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getAllProofOfResidenceOrBankStatements/{driverId}")]
        public async Task<IActionResult> GetAllProofOfResidenceOrBankStatements(Guid driverId)
        {
            var files = await _repoUnitOfWorks.driverRepo.GetAllProofOfResidenceOrBankStatements(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            //if (files == null || !files.Any())
            //{
            //    return NotFound("No proof of residence or bank statement for this driver");
            //}

            var response = files.Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileType,
                DownloadLink = Url.Action("DownLoadProofOfResidenceOrBankStatement", new { id = x.Id }) // Generate a download link

            });
            return Ok(response);
        }

        [HttpGet("downLoadProofOfResidenceOrBankStatement/{id}")]
        public async Task<IActionResult> DownLoadProofOfResidenceOrBankStatement(Guid id)
        {
            var file = await _repoUnitOfWorks.driverRepo.GetProofOfResidenceOrBankStatement(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            if (file == null)
            {
                return NotFound("No file found");
            }
            return File(file.FileData, file.FileType, file.FileName);
        }

        [HttpDelete("deleteProofOfResidenceOrBankStatement/{id}")]
        public async Task<IActionResult> DeleteProofOfResidenceOrBankStatement(Guid id)
        {
            await _repoUnitOfWorks.driverRepo.DeleteProofOfResidenceOrBankStatements(id);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("delete");
        }
        [HttpGet("getDriverIdByUserId/{userId}")]
        [AllowAnonymous] // Or secure it as needed
        public async Task<IActionResult> GetDriverIdByUserId(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound("User not found");

            var driver = await _repoUnitOfWorks.driverRepo.GetFirstDriverProfile(user.Email);
            if (driver == null) return NotFound("Driver profile not found");

            return Ok(driver.Id);
        }
        [HttpGet("getAvailableCarsForDriver")]
        public async Task<IActionResult> GetAvailableCarsForDriver()
        {
            try
            {
                var user = _identityService.CurrentUser;
                var driver = await _repoUnitOfWorks.driverRepo.GetFirstDriverProfile(user.Email);
                if (driver == null) return NotFound("Driver not found");

                var rentedCarIds = await _repoUnitOfWorks.chatRepo.GetCarIdsRentedByOtherDrivers(driver.Id.Value);

                var allCars = await _repoUnitOfWorks.ownerRepo.GetAllVehicles();

                var availableCars = allCars.Where(car => !rentedCarIds.Contains(car.Id)).ToList();

                return Ok(availableCars);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching available cars", ex.Message });
            }
        }
    }
}
