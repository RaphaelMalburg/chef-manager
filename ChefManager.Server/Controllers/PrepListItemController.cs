using ChefManager.Server.Data;
using ChefManager.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChefManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreplistItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PreplistItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreplistItem>>> GetAllPreplistItems()
        {
            var preplistItems = await _context.PreplistItems.ToListAsync();
            return Ok(preplistItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PreplistItem>> GetPreplistItem([FromRoute] int id)
        {
            var preplistItem = await _context.PreplistItems.FindAsync(id);
            if (preplistItem == null)
            {
                return NotFound();
            }
            return Ok(preplistItem);
        }

        [HttpPost]
        public async Task<ActionResult<PreplistItem>> CreatePreplistItem([FromBody] PreplistItem preplistItem)
        {
            _context.PreplistItems.Add(preplistItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPreplistItem", new { id = preplistItem.Id }, preplistItem);
        }

        [HttpPost("multiple")]
        [SwaggerOperation(Summary = "Create multiple preplistItems.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Returns the created preplistItems.", typeof(IEnumerable<PreplistItem>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid.")]
        public async Task<ActionResult<IEnumerable<PreplistItem>>> CreateMultiplePreplistItems([FromBody] List<PreplistItem> preplistItems)
        {
            // Validation
            var validationErrors = new List<string>();
            foreach (var item in preplistItems)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    validationErrors.Add("Preplist Item Name cannot be empty");
                }
                if (item.Amount <= 0)
                {
                    validationErrors.Add("Preplist Item Amount must be greater than 0");
                }
                if (item.PreplistId <= 0)
                {
                    validationErrors.Add("Preplist Id must be greater than 0");
                }
                if (item.UnitMeasureId <= 0)
                {
                    validationErrors.Add("Unit Measure Id must be greater than 0");
                }
            }

            // Handle validation errors
            if (validationErrors.Any())
            {
                return BadRequest(new { errors = validationErrors });
            }

            // Add items and save changes with error handling
            try
            {
                _context.PreplistItems.AddRange(preplistItems);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                // Log the exception details for further investigation
                return StatusCode(500, "An error occurred while saving preplist items");
            }

            return CreatedAtAction("GetAllPreplistItems", preplistItems); // return the created items
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PreplistItem>> UpdatePreplistItem([FromRoute] int id, [FromBody] PreplistItem preplistItem)
        {
            if (id != preplistItem.Id)
            {
                return BadRequest();
            }
            _context.Entry(preplistItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PreplistItem>> DeletePreplistItem([FromRoute] int id)
        {
            var preplistItem = await _context.PreplistItems.FindAsync(id);
            if (preplistItem == null)
            {
                return NotFound();
            }
            _context.PreplistItems.Remove(preplistItem);
            await _context.SaveChangesAsync();
            return Ok(preplistItem);
        }
    }
}
