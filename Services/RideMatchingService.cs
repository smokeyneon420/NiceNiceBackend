using nicenice.Server.NiceNiceDb;
using nicenice.Server.Models;
using Microsoft.EntityFrameworkCore;

public class RideMatchingService
{
    private readonly NiceNiceDbContext _context;

    public RideMatchingService(NiceNiceDbContext context)
    {
        _context = context;
    }

    public async Task<PassengerDriver> FindNearestDriverAsync(string pickupLocation)
    {
        return await _context.PassengerDrivers
            .OrderByDescending(d => d.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
