using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;

namespace nicenice.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly NiceNiceDbContext _context;

        public InvoiceController(NiceNiceDbContext context)
        {
            _context = context;
        }

        // GET: api/invoice
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _context.Invoices.ToListAsync();
            return Ok(invoices);
        }
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoice)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine("🚨 Invoice creation failed: " + ex.Message);
                return StatusCode(500, "Server error: " + ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] Invoice updatedInvoice)
        {
            if (id != updatedInvoice.InvoiceId)
                return BadRequest("Invoice ID mismatch");

            var existingInvoice = await _context.Invoices.FindAsync(id);
            if (existingInvoice == null)
                return NotFound();

            // Update fields
            existingInvoice.Amount = updatedInvoice.Amount;
            existingInvoice.WeekStart = updatedInvoice.WeekStart;
            existingInvoice.WeekEnd = updatedInvoice.WeekEnd;
            existingInvoice.Status = updatedInvoice.Status;
            existingInvoice.Description = updatedInvoice.Description;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingInvoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine("🚨 Invoice update failed: " + ex.Message);
                return StatusCode(500, "Server error: " + ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                return NotFound();

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
