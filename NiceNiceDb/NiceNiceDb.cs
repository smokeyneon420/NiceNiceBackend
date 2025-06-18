using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using nicenice.Server.Models;

namespace nicenice.Server.NiceNiceDb
{
    public class NiceNiceDbContext : IdentityDbContext<NiceUser, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public static string Auth = "auth";

        public NiceNiceDbContext(DbContextOptions<NiceNiceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("dbo");
            builder.Entity<NiceUser>().ToTable($"Users", Auth);
            builder.Entity<Role>().ToTable($"{nameof(Role)}s", Auth);
            builder.Entity<UserClaim>().ToTable($"{nameof(UserClaim)}s", Auth);
            builder.Entity<UserRole>().ToTable($"{nameof(UserRole)}s", Auth);
            builder.Entity<UserLogin>().ToTable($"{nameof(UserLogin)}s", Auth);
            builder.Entity<RoleClaim>().ToTable($"{nameof(RoleClaim)}s", Auth);
            builder.Entity<UserToken>().ToTable($"{nameof(UserToken)}s", Auth);
            builder.Entity<Drivers>().ToTable("Drivers", "Drivers"); // specify your desired schema here
            builder.Entity<Profilephotos>().ToTable("Profilephotos", "Drivers"); // specify your desired schema here
            builder.Entity<IdentityDocuments>().ToTable("IdentityDocuments", "Drivers"); // specify your desired schema here
            builder.Entity<LicenseCopies>().ToTable("LicenseCopies", "Drivers"); // specify your desired schema here
            builder.Entity<FingerPrintReports>().ToTable("FingerPrintReports", "Drivers"); // specify your desired schema here
            builder.Entity<CurrentPlatformScreenshots>().ToTable("CurrentPlatformScreenshots", "Drivers"); // specify your desired schema here
            builder.Entity<SelfiePics>().ToTable("SelfiePics", "Drivers"); // specify your desired schema here
            builder.Entity<Passports>().ToTable("Passports", "Drivers"); // specify your desired schema here
            builder.Entity<PermitOrAsylumLetters>().ToTable("PermitOrAsylumLetters", "Drivers"); // specify your desired schema here
            builder.Entity<BankConfirmationLetters>().ToTable("BankConfirmationLetters", "Drivers"); // specify your desired schema here
            builder.Entity<ProofOfResidenceOrBankStatements>().ToTable("ProofOfResidenceOrBankStatements", "Drivers"); // specify your desired schema here
            builder.Entity<Owner>().ToTable("Owners", "Owners"); // specify your desired schema here
            builder.Entity<FleetSupplier>().ToTable("FleetSuppliers", schema: "Owners");
            builder.Entity<VehicleType>().ToTable("VehicleTypes", schema: "dbo");
            builder.Entity<OwnerIdentityDocuments>().ToTable("OwnerIdentityDocuments", "Owners"); // specify your desired schema here
            builder.Entity<VehicleRegistrationDocuments>().ToTable("VehicleRegistrationDocuments", "Owners"); // specify your desired schema here
            builder.Entity<VehicleLicenseDiskAndOperatingCard>().ToTable("VehicleLicenseDiskAndOperatingCard", "Owners"); // specify your desired schema here
            builder.Entity<ProofOfInsurance>().ToTable("ProofOfInsurance", "Owners"); // specify your desired schema here
            builder.Entity<CarPictures>().ToTable("CarPictures", "Owners"); // specify your desired schema here
            builder.Entity<Cars>().ToTable("Cars", "Owners");
            builder.Entity<BankingDetail>().ToTable("BankingDetails", "Owners");
            builder.Entity<Passengers>().ToTable("Passengers", schema: "Passengers");
            builder.Entity<PassengersCar>().ToTable("Cars", "Passengers");
            builder.Entity<Cars>().HasOne(c => c.CarType)
            .WithMany()
            .HasForeignKey(c => c.CarTypeId)
            .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<FleetSupplierCar>()
            .ToTable("FleetSupplierCars", schema: "dbo")
            .HasOne(f => f.FleetSupplier)
            .WithMany(fs => fs.Cars)
            .HasForeignKey(f => f.FleetSupplierId);
            builder.Entity<Invoice>().ToTable("Invoices", "dbo");
            builder.Entity<Invoice>().Property(i => i.Amount).HasPrecision(18, 2);
            builder.Entity<VehiclePricing>().ToTable("VehiclePricing", "dbo");
            builder.Entity<VehiclePricing>().HasData(
                new VehiclePricing { Id = 1, VehicleType = "Hatchback", WeeklyRental = 2000, Deposit = 7500, MinContractMonths = 36, MaxContractMonths = 48 },
                new VehiclePricing { Id = 2, VehicleType = "Sedan", WeeklyRental = 2500, Deposit = 7500, MinContractMonths = 36, MaxContractMonths = 48 },
                new VehiclePricing { Id = 3, VehicleType = "SUV", WeeklyRental = 3500, Deposit = 10000, MinContractMonths = 48, MaxContractMonths = 60 },
                new VehiclePricing { Id = 4, VehicleType = "Minibus Taxi", WeeklyRental = 4500, Deposit = 10000, MinContractMonths = 60, MaxContractMonths = 60 },
                new VehiclePricing { Id = 5, VehicleType = "Electric Bike", WeeklyRental = 1300, Deposit = 1400, MinContractMonths = 48, MaxContractMonths = 48 }
            );
            builder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chats", "dbo");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.IsUnlocked).HasDefaultValue(false);
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(c => c.Driver)
                    .WithMany()
                    .HasForeignKey(c => c.DriverId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Car)
                    .WithMany()
                    .HasForeignKey(c => c.CarId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Owner)
                    .WithMany()
                    .HasForeignKey(c => c.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments", "dbo");
                entity.Property(p => p.AmountPaid).HasPrecision(18, 2);
                entity.Property(p => p.AdminFee).HasPrecision(18, 2);
                entity.Property(p => p.WeeklyRentalAmount).HasPrecision(18, 2);
                entity.Property(p => p.Deposit).HasPrecision(18, 2);

                entity.HasOne(p => p.Car)
                    .WithMany()
                    .HasForeignKey(p => p.CarId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Ride>().ToTable("Rides", "Passengers");
            builder.Entity<Ride>()
                .HasOne(r => r.Passenger)
                .WithMany()
                .HasForeignKey(r => r.PassengerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Ride>()
                .HasOne(r => r.Driver)
                .WithMany()
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<DriverLocation>().ToTable("DriverLocations", "Passengers");
            builder.Entity<DriverLocation>()
                .HasKey(dl => dl.Id);

            builder.Entity<DriverLocation>()
                .HasOne(dl => dl.Driver)
                .WithMany()
                .HasForeignKey(dl => dl.DriverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DriverLocation>()
                .HasOne(dl => dl.Ride)
                .WithMany()
                .HasForeignKey(dl => dl.RideId)
                .OnDelete(DeleteBehavior.Cascade);

        }
        public DbSet<Drivers> Drivers { get; set; }
        public DbSet<Profilephotos> Profilephotos { get; set; }
        public DbSet<OwnerIdentityDocuments> OwnerIdentityDocuments { get; set; }
        public DbSet<LicenseCopies> LicenseCopies { get; set; }
        public DbSet<FingerPrintReports> FingerPrintReports { get; set; }
        public DbSet<CurrentPlatformScreenshots> CurrentPlatformScreenshots { get; set; }
        public DbSet<SelfiePics> SelfiePics { get; set; }
        public DbSet<Passports> Passports { get; set; }
        public DbSet<PermitOrAsylumLetters> PermitOrAsylumLetters { get; set; }
        public DbSet<BankConfirmationLetters> BankConfirmationLetters { get; set; }
        public DbSet<ProofOfResidenceOrBankStatements> ProofOfResidenceOrBankStatements { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<IdentityDocuments> IdentityDocuments { get; set; }
        public DbSet<VehicleRegistrationDocuments> VehicleRegistrationDocuments { get; set; }
        public DbSet<VehicleLicenseDiskAndOperatingCard> VehicleLicenseDiskAndOperatingCard { get; set; }
        public DbSet<ProofOfInsurance> ProofOfInsurance { get; set; }
        public DbSet<CarPictures> CarPictures { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Provinces> Provinces { get; set; }
        public DbSet<Cars> Cars { get; set; }
        public DbSet<Town> Town { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<CarTypes> CarTypes { get; set; }
        public DbSet<CarColors> CarColors { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<VehiclePricing> VehiclePricings { get; set; }
        public DbSet<TermsAndConditions> TermsAndConditions { get; set; }
        public DbSet<BankingDetail> BankingDetails { get; set; }
        public DbSet<FleetSupplier> FleetSuppliers { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<FleetSupplierCar> FleetSupplierCars { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Passengers> Passengers { get; set; }
        public DbSet<PassengerDriver> PassengerDrivers { get; set; }
        public DbSet<PassengersCar> PassengersCars { get; set; }
        public DbSet<DriverLocation> DriverLocations { get; set; }
    }
}
