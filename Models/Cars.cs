using System.ComponentModel.DataAnnotations.Schema;

namespace nicenice.Server.Models
{
    public class Cars
    {
        public Guid? Id { get; set; }
        public Guid? OwnerId { get; set; }
        public int? ColorId { get; set; }
        public int? CarTypeId { get; set; }
        public CarTypes? CarType { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Name { get; set; }
        public string? Model { get; set; }
        public DateTime? RegistrationExpirydate { get; set; }
        public string? Mileage { get; set; }
        public int? PlatformId { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime Created { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? Updated { get; set; }
        public string? User { get; set; }
        public string? WeeklyRental { get; set; }
        public string? Make { get; set; }
        public int? Year { get; set; }
        public string? DriverAssignee { get; set; }
        public DateTime? DateAssigned { get; set; }
        public virtual ICollection<CarPictures> CarPictures { get; set; } = new List<CarPictures>();
        [NotMapped]
        public string? CarOwnerName { get; set; }
        [NotMapped]
        public string? DriverAssigned { get; set; }
    }
}
