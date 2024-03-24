using ChefManager.Server.Data;
using ChefManager.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChefManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreplistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PreplistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Preplist>>> GetAllPreplists()
        {
            var preplists = await _context.Preplists.ToListAsync();
            return Ok(preplists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Preplist>> GetPreplist([FromRoute] int id)
        {
            var preplist = await _context.Preplists.FindAsync(id);
            if (preplist == null)
            {
                return NotFound();
            }
            return Ok(preplist);
        }

        [HttpPost]
        public async Task<ActionResult<Preplist>> CreatePreplist([FromBody] Preplist preplist)
        {
            _context.Preplists.Add(preplist);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPreplist", new { id = preplist.Id }, preplist);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Preplist>> UpdatePreplist([FromRoute] int id, [FromBody] Preplist preplist)
        {
            if (id != preplist.Id)
            {
                return BadRequest();
            }
            _context.Entry(preplist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Preplist>> DeletePreplist([FromRoute] int id)
        {
            var preplist = await _context.Preplists.FindAsync(id);
            if (preplist == null)
            {
                return NotFound();
            }
            _context.Preplists.Remove(preplist);
            await _context.SaveChangesAsync();
            return Ok(preplist);
        }
    }
}
