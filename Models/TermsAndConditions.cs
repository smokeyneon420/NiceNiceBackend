using System;

namespace nicenice.Server.Models
{
    public class TermsAndConditions
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Version { get; set; }
    }
}
