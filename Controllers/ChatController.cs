using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using nicenice.Server.Data.UnitOfWorks;
using nicenice.Server.Models;
using System.Linq;
using nicenice.Server.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IRepoUnitOfWorks _repo;

    public ChatController(IRepoUnitOfWorks repo)
    {
        _repo = repo;
    }

    [HttpPost("unlockChat")]
    public async Task<IActionResult> UnlockChat([FromBody] ChatUnlockRequest request)
    {
        if (request.DriverId == Guid.Empty || request.CarId == Guid.Empty)
            return BadRequest("Invalid driver or car ID.");

        var car = await _repo.ownerRepo.GetCarById(request.CarId);
        if (car == null)
            return NotFound("Car not found.");

        var existingChat = await _repo.chatRepo.GetChatByDriverAndCar(request.DriverId, request.CarId);

        if (existingChat != null)
        {
            if (existingChat.IsUnlocked)
                return Ok("Chat already unlocked.");

            existingChat.IsUnlocked = true;
            await _repo.SaveChangesAsync();
            return Ok("Chat was locked but now unlocked.");
        }

        var newChat = new Chat
        {
            DriverId = request.DriverId,
            CarId = request.CarId,
            OwnerId = car.OwnerId ?? Guid.Empty,  // ✅ Ensure non-null (your DB constraint requires it)
            IsUnlocked = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.chatRepo.AddChat(newChat);
        await _repo.SaveChangesAsync();

        return Ok("Chat unlocked successfully.");
    }

    [HttpGet("isUnlocked")]
    public async Task<IActionResult> IsChatUnlocked([FromQuery] Guid driverId, [FromQuery] Guid carId)
    {
        if (driverId == Guid.Empty || carId == Guid.Empty)
            return BadRequest("Invalid driver or car ID.");

        var chat = await _repo.chatRepo.GetChatByDriverAndCar(driverId, carId);
        return Ok(chat != null && chat.IsUnlocked);
    }

    [HttpGet("getMessages")]
    public async Task<IActionResult> GetMessages([FromQuery] Guid carId, [FromQuery] Guid driverId)
    {
        if (carId == Guid.Empty || driverId == Guid.Empty)
            return BadRequest("Missing parameters.");

        var chat = await _repo.chatRepo.GetChatByDriverAndCar(driverId, carId);
        if (chat == null)
            return NotFound("Chat not found.");

        var messages = await _repo.chatRepo.GetMessagesByChatId(chat.Id);

        return Ok(messages.OrderBy(m => m.SentAt));
    }

    [HttpGet("getChatsForOwner/{ownerId}")]
    public async Task<IActionResult> GetChatsForOwner(Guid ownerId)
    {
        var chats = await _repo.chatRepo.GetChatsForOwner(ownerId);

        var chatDtos = chats.Select(chat => new ChatDto
        {
            Id = chat.Id,
            CarId = chat.CarId,
            IsUnlocked = chat.IsUnlocked,
            CreatedAt = chat.CreatedAt
        }).ToList();

        return Ok(chatDtos);
    }

    [HttpGet("getMessagesForOwner")]
    public async Task<IActionResult> GetMessagesForOwner([FromQuery] Guid carId, [FromQuery] Guid ownerId)
    {
        if (carId == Guid.Empty || ownerId == Guid.Empty)
            return BadRequest("Missing parameters.");

        var chat = await _repo.chatRepo.GetChatsForOwner(ownerId);
        var targetChat = chat.FirstOrDefault(c => c.CarId == carId);
        
        if (targetChat == null)
            return NotFound("Chat not found.");

        var messages = await _repo.chatRepo.GetMessagesByChatId(targetChat.Id);

        return Ok(messages.OrderBy(m => m.SentAt));
    }
}

public class ChatUnlockRequest
{
    public Guid DriverId { get; set; }
    public Guid CarId { get; set; }
}
