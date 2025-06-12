using System.ComponentModel.DataAnnotations.Schema;

namespace nicenice.Server.Models
{
    public class Drivers
    {
        public Guid? Id { get; set; }
        public string? YearsExperience { get; set; }
        public string? CriminalRecord { get; set; }
        public string? InsuranceClaim { get; set; }
        public int? NumberOfClaims { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public string? User { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? Telephone { get; set; }
        public bool? IsVerified { get; set; }
        [NotMapped]
        public string? CarAssigned { get; set; }
        public string? Gender { get; set; }
        public int? BuildingNumber { get; set; }
        public int? ProvinceId { get; set; }
        public string? Surbub { get; set; }
        public Guid? TownId { get; set; }
        public string? Street { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {Surname}";

    }
    public class Profilephotos
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }
    public class IdentityDocuments
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }
    public class LicenseCopies
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }
    public class FingerPrintReports
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }
    public class CurrentPlatformScreenshots
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }
    public class SelfiePics
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }
    public class Passports
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }
    public class PermitOrAsylumLetters
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }

    public class BankConfirmationLetters
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }
    public class ProofOfResidenceOrBankStatements
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid DriverId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[]? FileData { get; set; }     // The binary data for the file
        public string? FileName { get; set; }      // The name of the file
        public string? FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Drivers DriverProfile { get; set; }
    }
}
