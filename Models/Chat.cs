using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace nicenice.Server.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public NiceUser? Driver { get; set; }

        public Guid CarId { get; set; }
        public Cars? Car { get; set; }

        public Guid OwnerId { get; set; }
        public Owner? Owner { get; set; }

        public bool IsUnlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
