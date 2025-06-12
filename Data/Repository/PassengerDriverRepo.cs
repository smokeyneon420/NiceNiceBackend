using Microsoft.EntityFrameworkCore;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;
using nicenice.Server.Models.DTOs;

namespace nicenice.Server.Data.Repository
{
    public class PassengerDriverRepo : IPassengerDriverRepo
    {
        private readonly NiceNiceDbContext _niceniceDbContext;

        public PassengerDriverRepo(NiceNiceDbContext niceniceDbContext)
        {
            _niceniceDbContext = niceniceDbContext;
        }

        public async Task<bool> Exists(Guid userId)
        {
            return await _niceniceDbContext.PassengerDrivers.AnyAsync(p => p.UserId == userId);
        }

        public async Task<bool> Register(PassengerDriverDto model)
        {
            if (await Exists(model.UserId)) return false;

            var entity = new PassengerDriver
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                ContactNumber = model.ContactNumber,
                Gender = model.Gender,
                YearsExperience = model.YearsExperience,
                CreatedAt = DateTime.UtcNow
            };

            _niceniceDbContext.PassengerDrivers.Add(entity);
            return await _niceniceDbContext.SaveChangesAsync() > 0;
        }
    }
}
