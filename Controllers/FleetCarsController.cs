using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;
using Microsoft.EntityFrameworkCore;

namespace nicenice.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FleetCarsController : ControllerBase
    {
        private readonly NiceNiceDbContext _context;

        public FleetCarsController(NiceNiceDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFleetCars(Guid userId)
        {
            var authUser = await _context.Users.FindAsync(userId);
            if (authUser == null)
                return NotFound("User not found.");

            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.User == authUser.Email);

            var cars = await _context.Cars
                .Where(c => c.OwnerId == owner.Id)
                .Select(c => new
                {
                    c.Id,
                    c.Model,
                    c.Make,
                    c.Year,
                    c.RegistrationNumber,
                    c.WeeklyRental,
                    c.Created
                })
                .ToListAsync();

            return Ok(cars);
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddFleetCar([FromBody] AddFleetCarDto dto)
        {
            var authUser = await _context.Users.FindAsync(dto.UserId);
            if (authUser == null)
                return NotFound("User not found.");

            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.User == authUser.Email);

            if (owner == null)
                return BadRequest("Owner not found for this user.");

            var newCar = new Cars
            {
                Id = Guid.NewGuid(),
                OwnerId = owner.Id,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                RegistrationNumber = dto.RegistrationNumber,
                WeeklyRental = dto.WeeklyRental.ToString(),
                Created = DateTime.UtcNow
            };

            _context.Cars.Add(newCar);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Car added successfully." });
        }
    }

    public class AddFleetCarDto
    {
        public Guid UserId { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
        public string RegistrationNumber { get; set; } = "";
        public decimal WeeklyRental { get; set; }
    }
}
