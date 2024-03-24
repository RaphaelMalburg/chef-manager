using ChefManager.Server.Data;
using ChefManager.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;


namespace ChefManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitMeasuresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UnitMeasuresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UnitMeasures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitMeasure>>> GetUnitMeasures()
        {
            var unitMeasures = await _context.UnitMeasures.ToListAsync();
            return Ok(unitMeasures);
        }

        // GET: api/UnitMeasures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitMeasure>> GetUnitMeasure(int id)
        {
            var unitMeasure = await _context.UnitMeasures.FindAsync(id);
            if (unitMeasure == null)
            {
                return NotFound();
            }
            return Ok(unitMeasure);
        }

        // POST: api/UnitMeasures
        [HttpPost]
        public async Task<ActionResult<UnitMeasure>> CreateUnitMeasure([FromBody] UnitMeasure unitMeasure)
        {

            _context.UnitMeasures.Add(unitMeasure);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUnitMeasure", new { id = unitMeasure.Id }, unitMeasure);
        }
        //POST: api/UnitMeasures/multiple
        [HttpPost("multiple")]
        [SwaggerOperation(Summary = "Create multiple Unit measures.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Returns the created Unit measures.", typeof(IEnumerable<UnitMeasure>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid.")]
        public async Task<ActionResult<IEnumerable<UnitMeasure>>> CreateMultipleUnitMeasures([FromBody] List<UnitMeasure> unitMeasures)
        {
            // Validation
            var validationErrors = new List<string>();
            foreach (var item in unitMeasures)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    validationErrors.Add("Unit Measure Name cannot be empty");
                }
            }

            if (validationErrors.Any())
            {
                return BadRequest(new { errors = validationErrors });
            }
            try
            {
                _context.UnitMeasures.AddRange(unitMeasures);
                await _context.SaveChangesAsync();
           
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return CreatedAtAction("GetUnitMeasures", unitMeasures);

        }

        // PUT: api/UnitMeasures/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnitMeasure(int id, [FromBody] UnitMeasure unitMeasure)
        {
            if (id != unitMeasure.Id)
            {
                return BadRequest();
            }

            _context.Entry(unitMeasure).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/UnitMeasures/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<UnitMeasure>> DeleteUnitMeasure(int id)
        {
            var unitMeasure = await _context.UnitMeasures.FindAsync(id);
            if (unitMeasure == null)
            {
                return NotFound();
            }

            _context.UnitMeasures.Remove(unitMeasure);
            await _context.SaveChangesAsync();
            return Ok(unitMeasure);
        }
    }
}
