using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using nicenice.Server.Data.UnitOfWorks;
using nicenice.Server.Models;
using nicenice.Server.Models.DTOs;
using nicenice.Server.Services;
using nicenice.Server.NiceNiceDb;
using Microsoft.EntityFrameworkCore;

namespace nicenice.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverLocationController : ControllerBase
    {
        private readonly NiceNiceDbContext _context;

        public DriverLocationController(NiceNiceDbContext context)
        {
            _context = context;
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateLocation([FromBody] DriverLocationUpdateDto dto)
        {
            var existing = await _context.DriverLocations
                .FirstOrDefaultAsync(x => x.DriverId == dto.DriverId && x.RideId == dto.RideId);

            if (existing != null)
            {
                existing.Latitude = dto.Latitude;
                existing.Longitude = dto.Longitude;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                _context.DriverLocations.Add(new DriverLocation
                {
                    DriverId = dto.DriverId,
                    RideId = dto.RideId,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("get/{rideId}")]
        public async Task<IActionResult> GetLatestDriverLocation(Guid rideId)
        {
            var location = await _context.DriverLocations
                .Where(x => x.RideId == rideId)
                .OrderByDescending(x => x.UpdatedAt)
                .FirstOrDefaultAsync();

            if (location == null)
                return NotFound("No location found for this ride");

            return Ok(new
            {
                latitude = location.Latitude,
                longitude = location.Longitude,
                updatedAt = location.UpdatedAt
            });
        }
    }

}