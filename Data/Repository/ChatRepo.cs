using Microsoft.EntityFrameworkCore;
using nicenice.Server.Models;
using nicenice.Server.Models.DTOs;
using nicenice.Server.NiceNiceDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace nicenice.Server.Repository
{
    public class ChatRepo : IChatRepo
    {
        private readonly NiceNiceDbContext _context;

        public ChatRepo(NiceNiceDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> GetChatByDriverAndCar(Guid driverId, Guid carId)
        {
            return await _context.Chats
                .FirstOrDefaultAsync(c => c.DriverId == driverId && c.CarId == carId);
        }

        public async Task AddChat(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
        }

        public async Task<bool> ChatExists(Guid driverId, Guid carId)
        {
            return await _context.Chats.AnyAsync(c => c.DriverId == driverId && c.CarId == carId);
        }

        public async Task<bool> UnlockChat(Guid driverId, Guid carId)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.DriverId == driverId && c.CarId == carId);
            if (chat == null) return false;

            chat.IsUnlocked = true;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Message>> GetMessagesByChatId(Guid chatId)
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
        public async Task<IEnumerable<ChatDto>> GetChatsForOwner(Guid ownerId)
        {
            var chats = await _context.Chats
                .Where(c => c.OwnerId == ownerId)
                .ToListAsync();

            return chats.Select(chat => new ChatDto
            {
                Id = chat.Id,
                CarId = chat.CarId,
                IsUnlocked = chat.IsUnlocked,
                CreatedAt = chat.CreatedAt
            });
        }
        public async Task<List<Guid>> GetCarIdsRentedByOtherDrivers(Guid currentDriverId)
        {
            return await _context.Chats
                .Where(c => c.IsUnlocked && c.DriverId != currentDriverId)
                .Select(c => c.CarId)
                .Distinct()
                .ToListAsync();
        }
    }
}
