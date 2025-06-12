using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;

[ApiController]
[Route("api/[controller]")]
public class VehiclePricingController : ControllerBase
{
    private readonly NiceNiceDbContext _context;

    public VehiclePricingController(NiceNiceDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pricing = await _context.VehiclePricings.ToListAsync();
        return Ok(pricing);
    }

    [HttpGet("{vehicleType}")]
    public async Task<IActionResult> GetByType(string vehicleType)
    {
        var normalizedVehicleType = vehicleType.Replace(" ", "").ToLower();
        var item = await _context.VehiclePricings
            .FirstOrDefaultAsync(p => p.VehicleType.Replace(" ", "").ToLower() == normalizedVehicleType);
        
        return item != null ? Ok(item) : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] VehiclePricing updated)
    {
        var pricing = await _context.VehiclePricings.FindAsync(id);
        if (pricing == null) return NotFound();

        pricing.WeeklyRental = updated.WeeklyRental;
        pricing.Deposit = updated.Deposit;
        pricing.MinContractMonths = updated.MinContractMonths;
        pricing.MaxContractMonths = updated.MaxContractMonths;
        pricing.AdminFee = updated.AdminFee;

        await _context.SaveChangesAsync();
        return Ok(pricing);
    }
}
