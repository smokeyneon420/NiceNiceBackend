using nicenice.Server.Models;
using nicenice.Server.Models.DTOs;
using System;
using System.Threading.Tasks;


namespace nicenice.Server.Repository
{
    public interface IChatRepo
    {
        Task<Chat> GetChatByDriverAndCar(Guid driverId, Guid carId);
        Task AddChat(Chat chat);
        Task<bool> ChatExists(Guid driverId, Guid carId);
        Task<bool> UnlockChat(Guid driverId, Guid carId);
        Task<List<Message>> GetMessagesByChatId(Guid chatId);
        Task<IEnumerable<ChatDto>> GetChatsForOwner(Guid ownerId);
        Task<List<Guid>> GetCarIdsRentedByOtherDrivers(Guid currentDriverId);
    }
}
