
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nicenice.Server.Models;
using nicenice.Server.Models.DTOs;
using nicenice.Server.NiceNiceDb;
using nicenice.Server.Services;
using Microsoft.AspNetCore.Identity;

namespace nicenice.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FleetSuppliersController : ControllerBase
    {
        private readonly NiceNiceDbContext _context;
        private readonly IIdentityServices _identityServices;
         private readonly UserManager<NiceUser> _userManager;

        public FleetSuppliersController(NiceNiceDbContext context, IIdentityServices identityServices, UserManager<NiceUser> userManager)
        {
            _context = context;
            _identityServices = identityServices;
            _userManager = userManager;
        }
        [HttpPost("create-profile")]
        public async Task<IActionResult> CreateProfile([FromBody] FleetSupplierDto dto)
        {
            if (!dto.AcceptedTerms.GetValueOrDefault())
                return BadRequest("You must accept the LOI Terms and Conditions.");

            var profile = new FleetSupplier
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                FleetSupplierName = dto.FleetSupplierName,
                Address = dto.Address,
                BranchManagerName = dto.BranchManagerName,
                BranchManagerSurname = dto.BranchManagerSurname,
                CellNumber = dto.CellNumber,
                Email = dto.Email,
                VehicleTypeId = dto.VehicleTypeId,
                LOIFilePath = dto.LOIFilePath,
                AcceptedTerms = dto.AcceptedTerms.GetValueOrDefault(),
                TermsVersion = dto.TermsVersion
            };

            _context.FleetSuppliers.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Fleet Supplier profile created successfully" });
        }

        [HttpGet("exists/{userId}")]
        public async Task<IActionResult> ProfileExists(Guid userId)
        {
            var userEmail = await _userManager.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            var exists = await _context.FleetSuppliers.AnyAsync(fs => fs.Email == userEmail);
            return Ok(new { exists });
        }

        [HttpGet("get-vehicle-types")]
        public async Task<IActionResult> GetVehicleTypes()
        {
            var types = await _context.VehicleTypes
                .Select(v => new { v.Id, v.Name })
                .ToListAsync();

            return Ok(types);
        }

        [HttpPost("addCar")]
        public async Task<IActionResult> AddCar([FromBody] FleetCarDto model)
        {
            var userId = _identityServices.GetUserId();
            var fleetSupplier = await _context.FleetSuppliers
                .FirstOrDefaultAsync(fs => fs.UserId == userId);

            if (fleetSupplier == null)
                return BadRequest("Fleet Supplier profile not found.");

            var car = new FleetSupplierCar
            {
                Id = Guid.NewGuid(),
                FleetSupplierId = fleetSupplier.Id,
                ColorId = model.ColorId,
                CarTypeId = model.CarTypeId,
                RegistrationNumber = model.RegistrationNumber,
                Name = model.Name,
                Model = model.Model,
                Make = model.Make,
                Mileage = model.Mileage,
                PlatformId = model.PlatformId,
                WeeklyRental = model.WeeklyRental,
                RegistrationExpirydate = model.RegistrationExpirydate,
                Created = DateTime.UtcNow,
                IsApproved = false,
                LogoUrl = model.LogoUrl
            };

            _context.FleetSupplierCars.Add(car);
            await _context.SaveChangesAsync();

            return Ok("Car added successfully.");
        }
    }
}
