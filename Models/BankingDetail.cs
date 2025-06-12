using System;
using System.ComponentModel.DataAnnotations.Schema;
using nicenice.Server.Models;
public class BankingDetail
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string? AccountHolderName { get; set; }
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountType { get; set; }
    public string? BranchCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Owner? Owner { get; set; }
}
