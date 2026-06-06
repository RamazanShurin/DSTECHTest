using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.DataModels;
using static System.Net.Mime.MediaTypeNames;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeEquipmentsController : ControllerBase
    {
        private readonly TestContext _context;

        public TypeEquipmentsController(TestContext context)
        {
            _context = context;
        }

        // GET: api/TypeEquipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeEquipment>>> GetTypeEquipments()
        {
          if (_context.TypeEquipments == null)
          {
              return NotFound();
          }
            return await _context.TypeEquipments.ToListAsync();
        }

        // PUT: api/TypeEquipments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeEquipment(long id, string name, bool isActive)
        {
            var typeEquipment = await _context.TypeEquipments.FindAsync(id);
            if (typeEquipment == null)
            {
                return NotFound(new { error = "Error", message = "Не найдено оборудование" });
            }
            typeEquipment.Name = name;
            typeEquipment.IsActive = isActive;

            _context.Entry(typeEquipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeEquipmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TypeEquipments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TypeEquipment>> PostTypeEquipment(string name, bool isActive)
        {
            var typeEquipment = new TypeEquipment();
            typeEquipment.Name = name;
            typeEquipment.IsActive = isActive;
            _context.TypeEquipments.Add(typeEquipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/TypeEquipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeEquipment(long id)
        {
            var typeEquipment = await _context.TypeEquipments.FindAsync(id);
            if (typeEquipment == null)
            {
                return NotFound(new { error = "Error", message = "Не найдено оборудование" });
            }
            typeEquipment.IsActive = false;

            _context.Entry(typeEquipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeEquipmentExists(long id)
        {
            return (_context.TypeEquipments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
