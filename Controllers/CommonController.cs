using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using nicenice.Server.Data.UnitOfWorks;
using nicenice.Server.Models;
using nicenice.Server.Services;
using nicenice.Server.NiceNiceDb;
using Microsoft.EntityFrameworkCore;



namespace nicenice.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireLoggedIn")]
    public class CommonController : ControllerBase
    {
        private readonly IRepoUnitOfWorks _repoUnitOfWorks;
        private readonly IIdentityServices _identityService;
        private readonly UserManager<NiceUser> _userManager;
        private readonly NiceNiceDbContext _context;
        public CommonController(IRepoUnitOfWorks repoUnitOfWorks, UserManager<NiceUser> userManager, NiceNiceDbContext context)
        {
            _repoUnitOfWorks = repoUnitOfWorks;
            _userManager = userManager;
            _context = context;
            
        }
        [HttpGet]
        [Route("provinces")]
        public async Task<IActionResult> GetAllProvince()
        {
            try
            {
                var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllProvinces();
                var provinces = rowsAffected.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
                await _repoUnitOfWorks.SaveChangesAsync();
                if (provinces != null && provinces.Any())
                {
                    return Ok(provinces);
                }
                else
                {
                    // Return 404 if no provinces are found
                    return NotFound(new { message = "No provinces found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching provinces", error = ex.Message });

            }

        }
        [HttpGet("towns")]
        public async Task<IActionResult> GetAllTowns()
        {
            try
            {
                var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllTowns();
                var towns = rowsAffected.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                });
                await _repoUnitOfWorks.SaveChangesAsync();
                if (towns != null && towns.Any())
                {
                    return Ok(towns);
                }
                else
                {
                    // Return 404 if no provinces are found
                    return NotFound(new { message = "No towns found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching towns", error = ex.Message });

            }

        }
        [HttpGet("gender")]
        public async Task<IActionResult> GetAllGender()
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllGender();
            var gender = rowsAffected.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            });
            if (gender != null)
            {
                return Ok(gender);
            }
            else
            {
                return NotFound(new { message = "No gender found" });
            }

        }
        [HttpGet("allPlatform")]
        public async Task<IActionResult> GetAllPlatform()
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllPlatform();
            var platform = rowsAffected.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            });
            if (platform != null)
            {
                return Ok(platform);
            }
            else
            {
                return NotFound(new { message = "Not found" });
            }

        }
        [HttpGet("allCarTypes")]
        public async Task<IActionResult> GetAllCarTypes()
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllCarTypes();
            var carTypes = rowsAffected.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            });
            if (carTypes != null)
            {
                return Ok(carTypes);
            }
            else
            {
                return NotFound(new { message = "Not found" });
            }

        }
        [HttpGet("allCarColors")]
        public async Task<IActionResult> GetAllCarColors()
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllCarColors();
            var carColours = rowsAffected.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            });
            if (carColours != null)
            {
                return Ok(carColours);
            }
            else
            {
                return NotFound(new { message = "Not found" });
            }

        }
        [HttpGet("allCars")]
        public async Task<IActionResult> GetAllCars(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllCars(pageNumber, pageSize, searchTerm);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(rowsAffected);
        }
        [HttpGet("allDrivers")]
        public async Task<IActionResult> GetAllDrivers(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllDrivers(pageNumber, pageSize, searchTerm);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(rowsAffected);
        }
        [HttpGet("allOwners")]
        public async Task<IActionResult> GetAllOwners(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllOwners(pageNumber, pageSize, searchTerm);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(rowsAffected);
        }
        [HttpGet("getFirstDriver/{driverId}")]
        public async Task<IActionResult> GetFirstDriver(Guid driverId)
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetFirstDriver(driverId);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(rowsAffected);
        }

        [HttpGet("getFirstOwner/{ownerId}")]
        public async Task<IActionResult> GetFirstOwner(Guid ownerId)
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetFirstOwner(ownerId);
            //Console.WriteLine(rowsAffected);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(rowsAffected);
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            var rowsAffected = await _repoUnitOfWorks.commonRepo.GetAllUsers(pageNumber, pageSize, searchTerm);
            //Console.WriteLine(rowsAffected);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok(rowsAffected);
        }
        [HttpPut("updateDriver")]
        public async Task<IActionResult> UpdateFirstDriverProfile([FromBody] Drivers model)
        {
            var userUpdate = await _userManager.FindByEmailAsync(model.Email);
            if (userUpdate == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Update user properties
            userUpdate.FirstName = model.FirstName;
            userUpdate.LastName = model.Surname;
            userUpdate.UserName = model.Email;
            userUpdate.Email = model.Email;
            userUpdate.PhoneNumber = model.Contact;
            // You can also update other properties if needed

            // Update the user in the database
            var updateResult = await _userManager.UpdateAsync(userUpdate);
            if (!updateResult.Succeeded)
            {
                // Handle errors from the update operation
                return BadRequest(updateResult.Errors);
            }
            model.Updated = DateTime.Now;
            model.User = model.Email;
            await _repoUnitOfWorks.commonRepo.UpdateFirstDriverProfile(model);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("successful");
        }
        [HttpPut("updateOwner")]
        public async Task<IActionResult> UpdateFirstOwnerProfile([FromBody] Owner model)
        {
            var userUpdate = await _userManager.FindByEmailAsync(model.Email);
            if (userUpdate == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Update user properties
            userUpdate.FirstName = model.FirstName;
            userUpdate.LastName = model.Surname;
            userUpdate.UserName = model.Email;
            userUpdate.Email = model.Email;
            userUpdate.PhoneNumber = model.Contact;
            // You can also update other properties if needed

            // Update the user in the database
            var updateResult = await _userManager.UpdateAsync(userUpdate);
            if (!updateResult.Succeeded)
            {
                // Handle errors from the update operation
                return BadRequest(updateResult.Errors);
            }
            model.Updated = DateTime.Now;
            model.User = model.Email;
            await _repoUnitOfWorks.commonRepo.UpdateFirstOwnerProfile(model);
            await _repoUnitOfWorks.SaveChangesAsync();
            return Ok("successful");
        }
        [HttpGet("getAllVehicleTypes")]
        public async Task<IActionResult> GetAllVehicleTypes()
        {
            var vehicleTypes = await _context.VehicleTypes
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = v.Name
                })
                .ToListAsync();

            return Ok(vehicleTypes);
        }
    }
}
