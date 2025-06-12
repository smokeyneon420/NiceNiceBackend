using Microsoft.EntityFrameworkCore;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;

namespace nicenice.Server.Data.Repository
{
    public class DriverRepo : IDriverRepo
    {
        private readonly NiceNiceDbContext _niceniceDbContext;

        public DriverRepo(NiceNiceDbContext niceniceDbContext)
        {
            _niceniceDbContext = niceniceDbContext;
        }
        public async Task AddDriverProfile(Drivers model)
        {
            await _niceniceDbContext.Set<Drivers>().AddAsync(model);
        }

        public async Task UpdateDriverProfile(Drivers model)
        {
            var driver = await _niceniceDbContext.Set<Drivers>().FirstOrDefaultAsync(d => d.Email == model.Email);


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
        public async Task<Drivers> GetFirstDriverProfile(string? email)
        {
            return await _niceniceDbContext.Drivers.FirstOrDefaultAsync(d => d.Email == email);

        }

        public async Task<int> GetDriversCount()
        {
            int count = await _niceniceDbContext.Drivers.CountAsync();
            return count;
        }

        public async Task AddProfilePhotos(Profilephotos model)
        {
            await _niceniceDbContext.Profilephotos.AddAsync(model);
        }
        public async Task<bool> ProfilePhotoExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.Profilephotos
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<Profilephotos>> GetAllProfilephotos(Guid driverId)
        {
            return await _niceniceDbContext.Profilephotos.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<Profilephotos> GetProfilephoto(Guid id)
        {
            return await _niceniceDbContext.Profilephotos.FindAsync(id);
        }
        public async Task DeleteProfilePhoto(Guid id)
        {
            var file = await _niceniceDbContext.Profilephotos.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.Profilephotos.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddIdentityDocuments(IdentityDocuments model)
        {
            await _niceniceDbContext.IdentityDocuments.AddAsync(model);
        }
        public async Task<bool> IdentityDocumentsExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.IdentityDocuments
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<IdentityDocuments>> GetAllIdentityDocuments(Guid driverId)
        {
            return await _niceniceDbContext.IdentityDocuments.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<IdentityDocuments> GetIdentityDocument(Guid id)
        {
            return await _niceniceDbContext.IdentityDocuments.FindAsync(id);
        }
        public async Task DeleteIdentityDocuments(Guid id)
        {
            var file = await _niceniceDbContext.IdentityDocuments.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.IdentityDocuments.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddLicenseCopies(LicenseCopies model)
        {
            await _niceniceDbContext.LicenseCopies.AddAsync(model);
        }
        public async Task<bool> LicenseCopiesExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.LicenseCopies
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<LicenseCopies>> GetAllLicenseCopies(Guid driverId)
        {
            return await _niceniceDbContext.LicenseCopies.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<LicenseCopies> GetLicenseCopy(Guid id)
        {
            return await _niceniceDbContext.LicenseCopies.FindAsync(id);
        }
        public async Task DeleteLicenseCopies(Guid id)
        {
            var file = await _niceniceDbContext.LicenseCopies.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.LicenseCopies.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddFingerPrintReports(FingerPrintReports model)
        {
            await _niceniceDbContext.FingerPrintReports.AddAsync(model);
        }
        public async Task<bool> FingerPrintReportsExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.FingerPrintReports
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<FingerPrintReports>> GetAllFingerPrintReports(Guid driverId)
        {
            return await _niceniceDbContext.FingerPrintReports.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<FingerPrintReports> GetFingerPrintReport(Guid id)
        {
            return await _niceniceDbContext.FingerPrintReports.FindAsync(id);
        }
        public async Task DeleteFingerPrintReports(Guid id)
        {
            var file = await _niceniceDbContext.FingerPrintReports.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.FingerPrintReports.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddCurrentPlatformScreenshots(CurrentPlatformScreenshots model)
        {
            await _niceniceDbContext.CurrentPlatformScreenshots.AddAsync(model);
        }
        public async Task<bool> CurrentPlatformScreenshotsExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.CurrentPlatformScreenshots
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<CurrentPlatformScreenshots>> GetAllCurrentPlatformScreenshots(Guid driverId)
        {
            return await _niceniceDbContext.CurrentPlatformScreenshots.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<CurrentPlatformScreenshots> GetCurrentPlatformScreenshot(Guid id)
        {
            return await _niceniceDbContext.CurrentPlatformScreenshots.FindAsync(id);
        }
        public async Task DeleteCurrentPlatformScreenshots(Guid id)
        {
            var file = await _niceniceDbContext.CurrentPlatformScreenshots.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.CurrentPlatformScreenshots.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddSelfiePics(SelfiePics model)
        {
            await _niceniceDbContext.SelfiePics.AddAsync(model);
        }
        public async Task<bool> SelfiePicsExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.SelfiePics
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<SelfiePics>> GetAllSelfiePics(Guid driverId)
        {
            return await _niceniceDbContext.SelfiePics.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<SelfiePics> GetSelfiePic(Guid id)
        {
            return await _niceniceDbContext.SelfiePics.FindAsync(id);
        }
        public async Task DeleteSelfiePics(Guid id)
        {
            var file = await _niceniceDbContext.SelfiePics.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.SelfiePics.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddPassports(Passports model)
        {
            await _niceniceDbContext.Passports.AddAsync(model);
        }
        public async Task<bool> PassportsExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.Passports
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<Passports>> GetAllPassports(Guid driverId)
        {
            return await _niceniceDbContext.Passports.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<Passports> GetPassport(Guid id)
        {
            return await _niceniceDbContext.Passports.FindAsync(id);
        }
        public async Task DeletePassports(Guid id)
        {
            var file = await _niceniceDbContext.Passports.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.Passports.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddPermitOrAsylumLetters(PermitOrAsylumLetters model)
        {
            await _niceniceDbContext.PermitOrAsylumLetters.AddAsync(model);
        }
        public async Task<bool> PermitOrAsylumLettersExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.PermitOrAsylumLetters
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<PermitOrAsylumLetters>> GetAllPermitOrAsylumLetters(Guid driverId)
        {
            return await _niceniceDbContext.PermitOrAsylumLetters.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<PermitOrAsylumLetters> GetPermitOrAsylumLetter(Guid id)
        {
            return await _niceniceDbContext.PermitOrAsylumLetters.FindAsync(id);
        }
        public async Task DeletePermitOrAsylumLetters(Guid id)
        {
            var file = await _niceniceDbContext.PermitOrAsylumLetters.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.PermitOrAsylumLetters.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddBankConfirmationLetters(BankConfirmationLetters model) => await _niceniceDbContext.BankConfirmationLetters.AddAsync(model);
        public async Task<bool> BankConfirmationLettersExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.BankConfirmationLetters
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<BankConfirmationLetters>> GetAllBankConfirmationLetters(Guid driverId)
        {
            return await _niceniceDbContext.BankConfirmationLetters.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<BankConfirmationLetters> GetBankConfirmationLetter(Guid id)
        {
            return await _niceniceDbContext.BankConfirmationLetters.FindAsync(id);
        }
        public async Task DeleteBankConfirmationLetters(Guid id)
        {
            var file = await _niceniceDbContext.BankConfirmationLetters.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.BankConfirmationLetters.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
        public async Task AddProofOfResidenceOrBankStatements(ProofOfResidenceOrBankStatements model)
        {
            await _niceniceDbContext.ProofOfResidenceOrBankStatements.AddAsync(model);
        }
        public async Task<bool> ProofOfResidenceOrBankStatementsExists(Guid driverId, string fileName)
        {
            return await _niceniceDbContext.ProofOfResidenceOrBankStatements
                .AnyAsync(p => p.DriverId == driverId && p.FileName == fileName);
        }

        public async Task<List<ProofOfResidenceOrBankStatements>> GetAllProofOfResidenceOrBankStatements(Guid driverId)
        {
            return await _niceniceDbContext.ProofOfResidenceOrBankStatements.Where(x => x.DriverId == driverId).ToListAsync();
        }
        public async Task<ProofOfResidenceOrBankStatements> GetProofOfResidenceOrBankStatement(Guid id)
        {
            return await _niceniceDbContext.ProofOfResidenceOrBankStatements.FindAsync(id);
        }
        public async Task DeleteProofOfResidenceOrBankStatements(Guid id)
        {
            var file = await _niceniceDbContext.ProofOfResidenceOrBankStatements.FindAsync(id);
            if (file != null)
            {
                _niceniceDbContext.ProofOfResidenceOrBankStatements.Remove(file);
            }
            else
            {
                throw new InvalidOperationException("No file Found");
            }

        }
    }
}
