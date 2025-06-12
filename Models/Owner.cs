using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nicenice.Server.Models
{
    public class Owner
    {
        public Guid? Id { get; set; }
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
        public long? IdNumber { get; set; }
        public bool? IsVerified { get; set; }
        public string? Gender { get; set; }

        public int? BuildingNumber { get; set; }
        public int? ProvinceId { get; set; }
        public string? Surbub { get; set; }
        public Guid? TownId { get; set; }
        [NotMapped]
        public int NumberOfCars { get; set; }
        [NotMapped
        ]public int NumberOfDrivers { get; set; }
        public string? Street { get; set; }
    }
    public class OwnerIdentityDocuments
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid OwnerId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[] FileData { get; set; }     // The binary data for the file
        public string FileName { get; set; }      // The name of the file
        public string FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        public virtual Owner OwnerProfile { get; set; }
    }
    public class VehicleRegistrationDocuments
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid OwnerId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[] FileData { get; set; }     // The binary data for the file
        public string FileName { get; set; }      // The name of the file
        public string FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        public virtual Owner OwnerProfile { get; set; }
    }
    public class VehicleLicenseDiskAndOperatingCard
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid OwnerId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[] FileData { get; set; }     // The binary data for the file
        public string FileName { get; set; }      // The name of the file
        public string FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        public virtual Owner OwnerProfile { get; set; }
    }
    public class ProofOfInsurance
    {
        public Guid Id { get; set; }              // Primary Key
        public Guid OwnerId { get; set; }       // Foreign Key referencing DriverProfile
        public byte[] FileData { get; set; }     // The binary data for the file
        public string FileName { get; set; }      // The name of the file
        public string FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        public virtual Owner OwnerProfile { get; set; }
    }
    public class CarPictures
    {
        public Guid? CarId { get; set; }
        public Guid? Id { get; set; }    // Foreign Key referencing DriverProfile
        public byte[] FileData { get; set; }     // The binary data for the file
        public string FileName { get; set; }      // The name of the file
        public string FileType { get; set; }      // The type of the file (e.g., "image/jpeg")
        public DateTime Created { get; set; }   // Timestamp of when the record was created

        // Optional: Navigation property for related DriverProfile
        //public virtual Cars cars { get; set; }
    }

    public class Platform
    {
        [Key] // Indicates that this property is the primary key

        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CarTypes
    {
        [Key] // Indicates that this property is the primary key

        public int Id { get; set; }


        public string Name { get; set; }
    }
    public class CarColors
    {
        [Key] // Indicates that this property is the primary key

        public int Id { get; set; }


        public string Name { get; set; }
    }
}
