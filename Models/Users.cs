using Microsoft.AspNetCore.Identity;

namespace nicenice.Server.Models
{
    public class Users
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Role { get; set; }
        public bool RememberMe { get; set; }
        public DateTime? DateOfCreation { get; set; }
    }
    public class User
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string Role { get; set; }

    }
    public class Login
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
    //public class ApplicationUser : IdentityUser
    //{
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //}
    public class NiceUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfCreation { get; set; }
    }
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; set; } // The list of items for the current page
        public int TotalRecords { get; set; } // The total number of records available

        public PaginatedResult(IEnumerable<T> data, int totalRecords)
        {
            Data = data;
            TotalRecords = totalRecords;
        }
    }
    public class Role : IdentityRole<Guid> { }
    public class UserClaim : IdentityUserClaim<Guid> { }
    public class UserRole : IdentityUserRole<Guid> { }
    public class UserLogin : IdentityUserLogin<Guid> { }
    public class RoleClaim : IdentityRoleClaim<Guid> { }
    public class UserToken : IdentityUserToken<Guid> { }
}
