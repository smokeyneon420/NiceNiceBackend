using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace nicenice.Server.Hubs
{
    public class LocationHub : Hub
    {
        public async Task JoinLocationGroup(string rideId)
        {
            Console.WriteLine($"✅ Added to group: {rideId}, connectionId: {Context.ConnectionId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, rideId);
        }

        public async Task LeaveLocationGroup(string rideId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, rideId);
        }

        public async Task SendDriverLocation(string rideId, double latitude, double longitude)
        {
            Console.WriteLine($"📤 Sending location to group {rideId}");
            await Clients.Group(rideId).SendAsync("ReceiveDriverLocation", new
            {
                latitude,
                longitude
            });
        }
    }
}