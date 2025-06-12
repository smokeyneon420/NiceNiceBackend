using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Data.UnitOfWorks;

namespace nicenice.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IRepoUnitOfWorks _repoUnitOfWorks;

        public AdminController(IRepoUnitOfWorks repoUnitOfWorks)
        {
            _repoUnitOfWorks = repoUnitOfWorks;
        }

        [HttpGet("getAllBankingDetails")]
        public async Task<IActionResult> GetAllBankingDetails()
        {
            var result = await _repoUnitOfWorks.ownerRepo.GetAllBankingDetails();
            return Ok(result);
        }
    }
}
