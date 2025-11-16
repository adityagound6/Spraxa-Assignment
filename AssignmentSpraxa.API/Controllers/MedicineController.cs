using AssignmentSpraxa.API.Dto.PostDto;
using AssignmentSpraxa.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSpraxa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly AssignmentSpraxContext _context;

        public MedicineController(AssignmentSpraxContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medicines = await _context.Medicines
                .OrderByDescending(m => m.CreatedOn)
                .ToListAsync();

            return Ok(medicines);
        }

        [Authorize(Roles = "User")]
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string src)
        {
            // Base query
            IQueryable<Medicine> query = _context.Medicines.OrderByDescending(m => m.CreatedOn);

            // Apply filter only when src is not empty
            if (!string.IsNullOrWhiteSpace(src))
            {
                src = src.Trim().ToLower();

                query = query.Where(x =>
                    x.Name.ToLower().Contains(src) ||
                    x.Company.ToLower().Contains(src) ||
                    x.MedicineId.ToString().ToLower().Contains(src)
                );
            }

            // Execute query
            var medicines = await query.ToListAsync();

            // If nothing found
            if (medicines == null || medicines.Count == 0)
                return NotFound("Medicines not found");

            return Ok(medicines);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
                return NotFound("Medicine not found.");

            return Ok(medicine);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MedicinePostDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicineExists = await _context.Medicines.Where(m => m.Name == model.Name).AnyAsync();
            if (medicineExists)
                return Conflict("Medicine with the same name already exists.");

            var medicine = new Medicine()
            {
                Company = model.Company,
                ExpiryDate = model.ExpiryDate,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            };

            model.CreatedOn = DateTime.UtcNow;
            await _context.Medicines.AddAsync(medicine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = medicine.MedicineId }, model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Medicine model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _context.Medicines.FindAsync(id);
            if (existing == null)
                return NotFound("Medicine not found.");

            existing.Name = model.Name;
            existing.Company = model.Company;
            existing.Price = model.Price;
            existing.ExpiryDate = model.ExpiryDate;
            existing.Stock = model.Stock;

            _context.Medicines.Update(existing);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
                return NotFound("Medicine not found.");

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
