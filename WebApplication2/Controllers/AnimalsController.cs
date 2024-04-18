namespace WebApplication2.Controllers;


using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly ClinicContext _context;

        public AnimalsController(ClinicContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals(string orderBy = "name")
        {
            var animals = _context.Animals.AsQueryable();

            switch (orderBy)
            {
                case "name":
                    animals = animals.OrderBy(a => a.Name);
                    break;
                case "description":
                    animals = animals.OrderBy(a => a.Description);
                    break;
                case "category":
                    animals = animals.OrderBy(a => a.Category);
                    break;
                case "area":
                    animals = animals.OrderBy(a => a.Area);
                    break;
                default:
                    return BadRequest("Invalid orderBy parameter. Use 'name', 'description', 'category', or 'area'.");
            }

            return await animals.ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound($"Animal with ID {id} is not found.");
            }

            return animal;
        }

     
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimal), new { id = animal.IdAnimal }, animal);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, Animal animal)
        {
            if (id != animal.IdAnimal)
            {
                return BadRequest();
            }

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Animals.Any(e => e.IdAnimal == id))
                {
                    _context.Animals.Add(animal);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetAnimal), new { id = animal.IdAnimal }, animal);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
