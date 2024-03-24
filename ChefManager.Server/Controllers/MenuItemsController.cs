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
    public class MenuItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenuItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetAllMenuItems()
        {
            var menuItems = await _context.MenuItems.ToListAsync();
            return Ok(menuItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItem([FromRoute] int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return Ok(menuItem);
        }

        [HttpPost]
        public async Task<ActionResult<MenuItem>> CreateMenuItem([FromBody] MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetMenuItem", new { id = menuItem.Id }, menuItem);
        }

        [HttpPost("multiple")]
        [SwaggerOperation(Summary = "Create multiple MenuItem.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Returns the created MenuItem.", typeof(IEnumerable<MenuItem>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid.")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> CreateMultipleMenuItems([FromBody] List<MenuItem> menuItems)
        {
            // Validation
            var validationErrors = new List<string>();
            foreach (var item in menuItems)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    validationErrors.Add("Menu Item Name cannot be empty");
                }
                if (item.Price <= 0)
                {
                    validationErrors.Add("Menu Item Price must be greater than 0");
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
                _context.MenuItems.AddRange(menuItems);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            { 

                Console.WriteLine(ex.Message);
            
            
                // Log the exception details for further investigation
                return StatusCode(500, "An error occurred while saving menu items");
            }

            return CreatedAtAction("GetAllMenuItems", menuItems); // return the created items
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MenuItem>> UpdateMenuItem([FromRoute] int id, [FromBody] MenuItem menuItem)
        {
            if (id != menuItem.Id)
            {
                return BadRequest();
            }
            _context.Entry(menuItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MenuItem>> DeleteMenuItem([FromRoute] int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return Ok(menuItem);
        }
    }
}
