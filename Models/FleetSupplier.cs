using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using nicenice.Server.Models;

namespace nicenice.Server.Models
{
    public class FleetSupplier
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? FleetSupplierName { get; set; }
        public string? Address { get; set; }
        public string? BranchManagerName { get; set; }
        public string? BranchManagerSurname { get; set; }
        public string? CellNumber { get; set; }
        public string? Email { get; set; }
        public int VehicleTypeId { get; set; } // Comma-separated list
        public string? LOIFilePath { get; set; }
        public bool AcceptedTerms { get; set; }
        public string? TermsVersion { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<FleetSupplierCar>? Cars { get; set; }

    }
}
