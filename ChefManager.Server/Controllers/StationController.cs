using ChefManager.Server.Data;
using ChefManager.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ChefManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> GetAllStations()
        {
            var stations = await _context.Stations.ToListAsync();
            return Ok(stations);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> GetStation([FromRoute] int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            return Ok(station);
        }

        [HttpPost("multiple")]
        [SwaggerOperation(Summary = "Create multiple stations.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Returns the created stations.", typeof(IEnumerable<Station>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid.")]

        public async Task<ActionResult<IEnumerable<Station>>> CreateMultipleStations([FromBody] List<Station> stations)
        {
            // Validation
            var validationErrors = new List<string>();
            foreach (var item in stations)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    validationErrors.Add("Station Name cannot be empty");
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
                _context.Stations.AddRange(stations);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetAllStations", stations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Station>> CreateStation([FromBody] Station station)
        {
            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetStation", new { id = station.Id }, station);
        }
        [HttpPut]
        public async Task<ActionResult<Station>> UpdateStation([FromBody] Station station)
        {
            _context.Entry(station).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Station>> DeleteStation([FromRoute] int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
            return Ok(station);
        }

    }
}
