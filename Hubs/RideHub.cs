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

    public async Task NotifyRideCancelled(string rideId)
    {
        await Clients.Group(rideId).SendAsync("ReceiveRideUpdate", new
        {
            RideId = rideId,
            Status = "cancelled"
        });
    }
}