public class FleetSupplierDto
{
    public Guid UserId { get; set; }
    public string? FleetSupplierName { get; set; }
    public string? Address { get; set; }
    public string? BranchManagerName { get; set; }
    public string? BranchManagerSurname { get; set; }
    public string? CellNumber { get; set; }
    public string? Email { get; set; }
    public int VehicleTypeId { get; set; }
    public string? LOIFilePath { get; set; }
    public bool? AcceptedTerms { get; set; }
    public string? TermsVersion { get; set; }
}
