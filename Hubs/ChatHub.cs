using Microsoft.AspNetCore.SignalR;
using nicenice.Server.Data;
using nicenice.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using nicenice.Server.NiceNiceDb;

namespace nicenice.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly NiceNiceDbContext _context;

        public ChatHub(NiceNiceDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string carId, string senderId, string message, string timestamp)
        {
            await Clients.Group(carId).SendAsync("ReceiveMessage", senderId, message, timestamp, carId);

            var chat = await _context.Chats
                .FirstOrDefaultAsync(c => c.CarId.ToString() == carId);

            if (chat != null)
            {
                var newMessage = new Message
                {
                    Id = Guid.NewGuid(),
                    ChatId = chat.Id,
                    SenderId = Guid.Parse(senderId),
                    MessageText = message,
                    SentAt = DateTime.TryParse(timestamp, out var parsedTime) ? parsedTime : DateTime.UtcNow
                };

                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();
            }
        }

        // Adds the user to the SignalR group based on car ID
        public async Task JoinCarGroup(string carId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, carId);
        }

        // Optional: removes the user from the group (not always needed)
        public async Task LeaveCarGroup(string carId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, carId);
        }

        // Typing indicator (bonus feature)
        public async Task Typing(string carId, string senderId, string role)
        {
            await Clients.Group(carId).SendAsync("UserTyping", senderId, role);
            await Clients.Group(carId).SendAsync("Typing", senderId, role); 
        }
    }
}