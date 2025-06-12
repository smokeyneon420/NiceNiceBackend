using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class RideHub : Hub
{
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
}