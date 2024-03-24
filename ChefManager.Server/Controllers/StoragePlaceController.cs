using ChefManager.Server.Data;

using ChefManager.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ChefManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoragePlacesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StoragePlacesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoragePlace>>> GetAllStoragePlaces()
        {
            var  storagePlaces = await _context.StoragePlaces.ToListAsync();
            return Ok(storagePlaces);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<StoragePlace>> GetStoragePlace([FromRoute] int id)
        {
            var storagePlace = await _context.StoragePlaces.FindAsync(id);
            if (storagePlace == null)
            {
                return NotFound();
            }
            return Ok(storagePlace);
        }
        [HttpPost("multiple")]
        [SwaggerOperation(Summary = "Create multiple storage places.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Returns the created storage places.", typeof(IEnumerable<StoragePlace>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid.")]
        public async Task<ActionResult<IEnumerable<StoragePlace>>> CreateMultipleStoragePlaces([FromBody] List<StoragePlace> storagePlace)
        {
            // Validation
            var validationErrors = new List<string>();
            foreach (var item in storagePlace)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    validationErrors.Add("Storage Place Name cannot be empty");
                }
                if (string.IsNullOrEmpty(item.Description))
                {
                    validationErrors.Add("Storage Place Description cannot be empty");
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
                _context.StoragePlaces.AddRange(storagePlace);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details for further investigation
                


                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while saving preplist items");
            }

            return CreatedAtAction("GetUnitMeasures", storagePlace); // return the created items
        }
        [HttpPost]
        public async Task<ActionResult<StoragePlace>> CreateStoragePlace([FromBody] StoragePlace storagePlace)
        {
            _context.StoragePlaces.Add(storagePlace);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetStoragePlace", new { id = storagePlace.Id }, storagePlace);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StoragePlace>> UpdateStoragePlace([FromRoute] int id, [FromBody] StoragePlace storagePlace)
        {
            if (id != storagePlace.Id)
            {
                return BadRequest();
            }
            // .entry .Entry(...).State = EntityState.Modified:
            //This line of code utilizes Entity Framework Core(EF Core) to update the storage place information in the database.
            //.Entry(storagePlace): This part tells EF Core to track the provided storagePlace object.
            //.State = EntityState.Modified: This sets the state of the tracked object to Modified, indicating that it needs to be updated in the database.

            _context.Entry(storagePlace).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<StoragePlace>> DeleteStoragePlace([FromRoute] int id)
        {
            var storagePlace = await _context.StoragePlaces.FindAsync(id);
            if (storagePlace == null)
            {
                return NotFound();
            }
            _context.StoragePlaces.Remove(storagePlace);
            await _context.SaveChangesAsync();
            return Ok(storagePlace);
        }

        

    }
   
}
