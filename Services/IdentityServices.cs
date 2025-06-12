using Microsoft.AspNetCore.Identity;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;

namespace nicenice.Server.Services
{
    public class IdentityServices : IIdentityServices
    {
        public readonly IHttpContextAccessor _httpContext;
        public readonly NiceNiceDbContext _niceniceDb;
        private readonly UserManager<NiceUser> _userManager;
        private NiceUser _currentUser;
        private string _currentRole;
        public IdentityServices(IHttpContextAccessor context, NiceNiceDbContext niceniceDb, UserManager<NiceUser> userManager)
        {
            _httpContext = context;
            _niceniceDb = niceniceDb;
            _userManager = userManager;
        }



        public NiceUser CurrentUser => _niceniceDb.Set<NiceUser>().FirstOrDefault(x => x.Email == _httpContext.HttpContext.User.Identity.Name);
        // Method to get the current user's role
        public async Task<string> GetCurrentUserRoleAsync()
        {
            var user = CurrentUser; // Use the existing property to get the current user

            if (user == null)
            {
                return null; // Handle the case where the user is not found
            }

            // Fetch the roles of the current user
            var roles = await _userManager.GetRolesAsync(user);

            // Return the first role or null if there are no roles
            return roles.FirstOrDefault();
        }
        public Guid? GetUserId()
        {
            var userIdStr = _httpContext.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdStr, out var guid) ? guid : (Guid?)null;
        }

    }
}
