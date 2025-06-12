using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Data.UnitOfWorks;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace nicenice.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengersController : ControllerBase
    {
        private readonly NiceNiceDbContext _context;

        public PassengersController(NiceNiceDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterPassenger([FromBody] PassengerRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var passenger = new Passengers
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ContactNumber = request.ContactNumber
            };

            _context.Passengers.Add(passenger);
            await _context.SaveChangesAsync();

            var response = new PassengerResponseDto
            {
                Id = passenger.Id,
                FullName = $"{passenger.FirstName} {passenger.LastName}",
                Email = passenger.Email
            };

            return Ok(response);
        }
        [HttpGet("exists/{userId}")]
        public async Task<IActionResult> CheckIfPassengerExists(Guid userId)
        {
            var exists = await _context.Passengers.AnyAsync(p => p.UserId == userId);
            return Ok(new { exists });
        }

        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetPassengerProfile(Guid userId)
        {
            var passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.UserId == userId);

            if (passenger == null)
                return NotFound("Passenger not found.");

            return Ok(new
            {
                passenger.FirstName,
                passenger.LastName,
                passenger.Email,
                passenger.ContactNumber,
                passenger.Created
            });
        }

        [HttpPost("updateProfile")]
        public async Task<IActionResult> UpdatePassengerProfile([FromBody] PassengerRequestDto request)
        {
            var passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.UserId == request.UserId);

            if (passenger == null)
                return NotFound("Passenger not found.");

            passenger.FirstName = request.FirstName ?? passenger.FirstName;
            passenger.LastName = request.LastName ?? passenger.LastName;
            passenger.Email = request.Email ?? passenger.Email;
            passenger.ContactNumber = request.ContactNumber ?? passenger.ContactNumber;

            await _context.SaveChangesAsync();
            return Ok("Profile updated successfully.");
        }
    }
}