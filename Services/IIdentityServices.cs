using nicenice.Server.Models;

namespace nicenice.Server.Services
{
    public interface IIdentityServices
    {
        NiceUser CurrentUser { get; }

        Task<string> GetCurrentUserRoleAsync();

        Guid? GetUserId();

    }
}
