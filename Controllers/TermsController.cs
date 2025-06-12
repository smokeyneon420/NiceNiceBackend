using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nicenice.Server.NiceNiceDb;

namespace nicenice.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TermsController : ControllerBase
    {
        private readonly NiceNiceDbContext _context;

        public TermsController(NiceNiceDbContext context)
        {
            _context = context;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            var latest = await _context.TermsAndConditions
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();

            if (latest == null) return NotFound("No terms found.");

            return Ok(latest);
        }
    }
}
