using nicenice.Server.Models;

namespace nicenice.Server.Data.Repository
{
    public interface IDriverRepo
    {
        Task AddDriverProfile(Drivers model);
        Task UpdateDriverProfile(Drivers model);
        //Task<IEnumerable<Owner>> GetOwnerProfileDetails();
        Task<Drivers> GetFirstDriverProfile(string? email);
        Task<int> GetDriversCount();
        Task AddProfilePhotos(Profilephotos model);
        Task AddIdentityDocuments(IdentityDocuments model);
        Task AddLicenseCopies(LicenseCopies model);
        Task AddFingerPrintReports(FingerPrintReports model);
        Task AddCurrentPlatformScreenshots(CurrentPlatformScreenshots model);
        Task AddSelfiePics(SelfiePics model);
        Task AddPassports(Passports model);
        Task AddPermitOrAsylumLetters(PermitOrAsylumLetters model);
        Task AddProofOfResidenceOrBankStatements(ProofOfResidenceOrBankStatements model);
        Task AddBankConfirmationLetters(BankConfirmationLetters model);
        Task<List<Profilephotos>> GetAllProfilephotos(Guid driverId);
        Task<List<IdentityDocuments>> GetAllIdentityDocuments(Guid driverId);
        Task<List<LicenseCopies>> GetAllLicenseCopies(Guid driverId);
        Task<List<FingerPrintReports>> GetAllFingerPrintReports(Guid driverId);
        Task<List<CurrentPlatformScreenshots>> GetAllCurrentPlatformScreenshots(Guid driverId);
        Task<List<SelfiePics>> GetAllSelfiePics(Guid driverId);
        Task<List<Passports>> GetAllPassports(Guid driverId);
        Task<List<PermitOrAsylumLetters>> GetAllPermitOrAsylumLetters(Guid driverId);
        Task<List<BankConfirmationLetters>> GetAllBankConfirmationLetters(Guid driverId);
        Task<List<ProofOfResidenceOrBankStatements>> GetAllProofOfResidenceOrBankStatements(Guid driverId);
        Task<Profilephotos> GetProfilephoto(Guid id);
        Task<IdentityDocuments> GetIdentityDocument(Guid id);
        Task<LicenseCopies> GetLicenseCopy(Guid id);
        Task<FingerPrintReports> GetFingerPrintReport(Guid id);
        Task<CurrentPlatformScreenshots> GetCurrentPlatformScreenshot(Guid id);
        Task<SelfiePics> GetSelfiePic(Guid id);
        Task<Passports> GetPassport(Guid id);
        Task<PermitOrAsylumLetters> GetPermitOrAsylumLetter(Guid id);
        Task<BankConfirmationLetters> GetBankConfirmationLetter(Guid id);
        Task<ProofOfResidenceOrBankStatements> GetProofOfResidenceOrBankStatement(Guid id);
        Task<bool> ProfilePhotoExists(Guid driverId, string fileName);
        Task<bool> IdentityDocumentsExists(Guid driverId, string fileName);
        Task<bool> LicenseCopiesExists(Guid driverId, string fileName);
        Task<bool> FingerPrintReportsExists(Guid driverId, string fileName);
        Task<bool> CurrentPlatformScreenshotsExists(Guid driverId, string fileName);
        Task<bool> SelfiePicsExists(Guid driverId, string fileName);
        Task<bool> PassportsExists(Guid driverId, string fileName);
        Task<bool> PermitOrAsylumLettersExists(Guid driverId, string fileName);
        Task<bool> BankConfirmationLettersExists(Guid driverId, string fileName);
        Task<bool> ProofOfResidenceOrBankStatementsExists(Guid driverId, string fileName);
        Task DeleteProfilePhoto(Guid id);
        Task DeleteIdentityDocuments(Guid id);
        Task DeleteLicenseCopies(Guid id);
        Task DeleteFingerPrintReports(Guid id);
        Task DeleteCurrentPlatformScreenshots(Guid id);
        Task DeleteSelfiePics(Guid id);
        Task DeletePassports(Guid id);
        Task DeletePermitOrAsylumLetters(Guid id);
        Task DeleteBankConfirmationLetters(Guid id);
        Task DeleteProofOfResidenceOrBankStatements(Guid id);
    }
}
