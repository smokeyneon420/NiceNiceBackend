using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Data.UnitOfWorks;
using nicenice.Server.Models;
using nicenice.Server.Services;
using nicenice.Server.NiceNiceDb;
using nicenice.Server.Models.DTOs;
using nicenice.Server.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace nicenice.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/passengerdrivers")]
    public class PassengerDriversController : ControllerBase
    {
        private readonly NiceNiceDbContext _context;
        private readonly IRepoUnitOfWorks _repoUnitOfWorks;
        private readonly IIdentityServices _identityService;
        private readonly UserManager<NiceUser> _userManager;
        private readonly CommonServices _commonServices;
        private readonly IHubContext<RideHub> _hubContext;

        public PassengerDriversController(NiceNiceDbContext context, IRepoUnitOfWorks repoUnitOfWorks, IIdentityServices identityService, UserManager<NiceUser> userManager, CommonServices commonServices, IHubContext<RideHub> hubContext)
        {
            _context = context;
            _repoUnitOfWorks = repoUnitOfWorks;
            _identityService = identityService;
            _userManager = userManager;
            _commonServices = commonServices;
            _hubContext = hubContext;
        }

        [HttpGet("exists/{userId}")]
        public async Task<IActionResult> Exists(Guid userId)
        {
            var exists = await _repoUnitOfWorks.passengerDriverRepo.Exists(userId);
            return Ok(new { exists });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(PassengerDriverDto model)
        {
            var result = await _repoUnitOfWorks.passengerDriverRepo.Register(model);
            if (!result)
                return BadRequest("Unable to register passenger-assigned driver.");

            return Ok(new { message = "Driver registered successfully." });
        }

        [HttpGet("getDriverIdByUserId/{userId}")]
        public async Task<IActionResult> GetDriverIdByUserId(Guid userId)
        {
            var driver = await _context.PassengerDrivers
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
                return NotFound("Driver profile not found.");

            return Ok(driver.Id);
        }

        [HttpGet("getProfile/{userId}")]
        public async Task<IActionResult> GetDriverProfile(Guid userId)
        {
            var driver = await _context.PassengerDrivers
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
                return NotFound("Driver not found.");

            return Ok(new
            {
                driver.FirstName,
                driver.LastName,
                driver.ContactNumber,
                driver.Gender,
                driver.YearsExperience,
                driver.Email,
                driver.CreatedAt,
                driver.Id,
                driver.UserId
            });
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _identityService.GetUserId();

            var driver = await _context.PassengerDrivers
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
                return NotFound("Driver profile not found.");

            return Ok(new
            {
                driver.FirstName,
                driver.LastName,
                Contact = driver.ContactNumber,
                driver.Gender,
                driver.YearsExperience,
                driver.ProfileImageUrl
            });
        }

        [HttpPost("updateProfile")]
        public async Task<IActionResult> UpdateDriverProfile([FromBody] DriverProfileUpdateDto dto)
        {
            var userId = _identityService.GetUserId();
            var driver = await _context.PassengerDrivers
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
                return NotFound("Driver not found.");

            driver.FirstName = dto.FirstName ?? driver.FirstName;
            driver.LastName = dto.LastName ?? driver.LastName;
            driver.ContactNumber = dto.ContactNumber ?? driver.ContactNumber;
            driver.Gender = dto.Gender ?? driver.Gender;
            driver.YearsExperience = dto.YearsExperience ?? driver.YearsExperience;

            await _context.SaveChangesAsync();
            return Ok("Profile updated successfully.");
        }

        [HttpPost("uploadProfileImage")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _identityService.GetUserId(); // get current logged-in user
            var driver = await _context.PassengerDrivers.FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
                return NotFound("Driver profile not found.");

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine("wwwroot/profile-images", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            driver.ProfileImageUrl = $"/profile-images/{fileName}";
            await _context.SaveChangesAsync();

            return Ok(new { imageUrl = driver.ProfileImageUrl });
        }

        [HttpPost("update-location")]
        public async Task<IActionResult> UpdateDriverLocation([FromBody] DriverLocationDto dto)
        {
            var userId = _identityService.GetUserId();

            if (userId == null || userId == Guid.Empty)
            {
                Console.WriteLine("❌ Could not extract user ID from token.");
                return Unauthorized("User not authenticated.");
            }

            var driverId = await _context.PassengerDrivers
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefaultAsync();

            if (driverId == Guid.Empty)
            {
                Console.WriteLine("❌ No driver found with this UserId.");
                return NotFound("Driver profile not found.");
            }

            var existingLocation = await _context.DriverLocations
                .FirstOrDefaultAsync(l => l.DriverId == driverId && l.RideId == dto.RideId);

            if (existingLocation != null)
            {
                existingLocation.Latitude = dto.Latitude;
                existingLocation.Longitude = dto.Longitude;
                existingLocation.Timestamp = DateTime.UtcNow;
            }
            else
            {
                _context.DriverLocations.Add(new DriverLocation
                {
                    Id = Guid.NewGuid(),
                    DriverId = driverId,
                    RideId = dto.RideId,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    Timestamp = DateTime.UtcNow
                });
            }

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("✅ Location updated.");
                return Ok(new { message = "Location stored." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return BadRequest("Error saving to DB: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("latest-location/{rideId}")]
        public async Task<IActionResult> GetLatestDriverLocation(Guid rideId)
        {
            var location = await _context.DriverLocations
                .Where(l => l.RideId == rideId)
                .Select(l => new
                {
                    l.Latitude,
                    l.Longitude,
                    l.Timestamp
                })
                .FirstOrDefaultAsync();

            if (location == null)
                return NotFound("No location found for this ride.");

            return Ok(location);
        }
    }
}