using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Data;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;

namespace nicenice.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengersCarsController : ControllerBase
    {
        private readonly NiceNiceDbContext _context;

        public PassengersCarsController(NiceNiceDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCar([FromBody] PassengersCar car)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.PassengersCars.Add(car);
            await _context.SaveChangesAsync();
            return Ok(car);
        }

        [HttpGet("by-driver/{driverId}")]
        public IActionResult GetCarsByDriver(Guid driverId)
        {
            var cars = _context.PassengersCars
                .Where(c => c.DriverId == driverId)
                .ToList();

            return Ok(cars);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] PassengersCar updatedCar)
        {
            var existingCar = await _context.PassengersCars.FindAsync(id);
            if (existingCar == null)
                return NotFound("Car not found.");

            existingCar.Make = updatedCar.Make;
            existingCar.Model = updatedCar.Model;
            existingCar.Year = updatedCar.Year;
            existingCar.PlateNumber = updatedCar.PlateNumber;
            existingCar.VehicleType = updatedCar.VehicleType;

            await _context.SaveChangesAsync();
            return Ok(existingCar);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.PassengersCars.FindAsync(id);
            if (car == null)
                return NotFound("Car not found.");

            _context.PassengersCars.Remove(car);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Car deleted successfully." });
        }
    }
}