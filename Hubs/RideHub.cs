using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using nicenice.Server.Data;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;

public class RideHub : Hub
{
    private readonly NiceNiceDbContext _context;

    public RideHub(NiceNiceDbContext context)
    {
        _context = context;
    }

    public async Task JoinRideGroup(string rideId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, rideId);
    }

    public async Task LeaveRideGroup(string rideId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, rideId);
    }

    public async Task JoinOnlineDriversGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "online-drivers");
    }

    public async Task LeaveOnlineDriversGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "online-drivers");
    }

    public async Task SendLiveDriverLocation(double latitude, double longitude)
    {
        await Clients.Group("online-drivers").SendAsync("ReceiveDriverLocation", new
        {
            Latitude = latitude,
            Longitude = longitude
        });
    }

    public async Task NotifyRideCancelled(string rideId)
    {
        await Clients.Group(rideId).SendAsync("ReceiveRideUpdate", new
        {
            RideId = rideId,
            Status = "cancelled"
        });
    }

    public async Task SendDriverLocationToRideGroup(Guid driverId, Guid rideId, double latitude, double longitude)
    {
        var existing = await _context.DriverLocations
            .FirstOrDefaultAsync(x => x.DriverId == driverId && x.RideId == rideId);

        if (existing != null)
        {
            existing.Latitude = latitude;
            existing.Longitude = longitude;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _context.DriverLocations.Add(new DriverLocation
            {
                DriverId = driverId,
                RideId = rideId,
                Latitude = latitude,
                Longitude = longitude,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();

        await Clients.Group(rideId.ToString()).SendAsync("ReceiveDriverLocation", new
        {
            rideId,
            latitude,
            longitude,
            timestamp = DateTime.UtcNow
        });
    }
}