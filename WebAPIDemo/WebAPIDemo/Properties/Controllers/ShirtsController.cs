
     using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Properties.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WebAPIDemo.Properties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShirtsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShirtsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShirts()
        {
            return Ok(await _context.Shirts.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShirtById(int id)
        {
            var shirt = await _context.Shirts.FindAsync(id);
            if (shirt == null)
                return NotFound($"Shirt with ID {id} not found.");
            return Ok(shirt);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShirt([FromBody] Shirt shirt)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Shirts.AddAsync(shirt);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetShirtById), new { id = shirt.ShirtId }, shirt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShirt(int id, [FromBody] Shirt shirt)
        {
            if (!ModelState.IsValid || shirt.ShirtId != id)
                return BadRequest(ModelState);

            var existingShirt = await _context.Shirts.FindAsync(id);
            if (existingShirt == null)
                return NotFound($"Shirt with ID {id} not found.");

            existingShirt.Name = shirt.Name;
            existingShirt.Color = shirt.Color;
            existingShirt.Size = shirt.Size;
            existingShirt.Price = shirt.Price;
            await _context.SaveChangesAsync();
            return Ok(existingShirt);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShirt(int id)
        {
            var shirt = await _context.Shirts.FindAsync(id);
            if (shirt == null)
                return NotFound($"Shirt with ID {id} not found.");

            _context.Shirts.Remove(shirt);
            await _context.SaveChangesAsync();
            return Ok($"Shirt with ID {id} deleted.");
        }
    }
}
  