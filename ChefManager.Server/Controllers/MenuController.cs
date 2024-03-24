using ChefManager.Server.Data;
using ChefManager.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChefManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetAllMenus()
        {
            var menus = await _context.Menus.ToListAsync();
            return Ok(menus);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenu([FromRoute] int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return Ok(menu);
        }

        [HttpPost]
        public async Task<ActionResult<Menu>> CreateMenu([FromBody] Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetMenu", new { id = menu.Id }, menu);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Menu>> UpdateMenu([FromRoute] int id, [FromBody] Menu menu)
        {
            if (id != menu.Id)
            {
                return BadRequest();
            }
            _context.Entry(menu).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Menu>> DeleteMenu([FromRoute] int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
            return Ok(menu);
        }
    }
}
