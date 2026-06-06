using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.DataModels;
using Test.DTOs;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly TestContext _context;

        public ApplicationsController(TestContext context)
        {
            _context = context;
        }

        // GET: api/Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationDTO>>> GetApplications(long? statusId = null, long? typeEquipmentId = null, string? subdivision = null, DateTime? createdDate = null, string? applicantFullName = null, int page = 1, int? pageSize = null)
        {
          if (_context.Applications == null)
          {
              return NotFound();
          }
            if (pageSize == null)
            {
                return await _context.Applications.Select(r =>  new ApplicationDTO { 
                    ApplicationNumber = "req-" + r.CreatedDate.Year.ToString() + "-" + r.Id, 
                    Id = r.Id,
                    ApplicationStatusId = r.ApplicationStatusId,
                    ApplicantFullName = r.ApplicantFullName,
                    Subdivision = r.Subdivision,
                    ApplicationStatus = r.ApplicationStatus.Name,
                    TypeEquipmentId = r.TypeEquipmentId,
                    TypeEquipment = r.TypeEquipment.Name,
                    Description = r.Description,
                    CreatedDate = r.CreatedDate,
                    UpdateDate = r.UpdateDate,
                    HandlerComment = r.HandlerComment
                }).Where(r =>
                    (statusId != null ? r.ApplicationStatusId == statusId : true) &&
                    (typeEquipmentId != null ? r.TypeEquipmentId == typeEquipmentId : true) &&
                    (subdivision != null ? r.Subdivision.ToLower() == subdivision.ToLower() : true) &&
                    (createdDate != null ? r.CreatedDate.Day == createdDate.Value.Day && r.CreatedDate.Month == createdDate.Value.Month && r.CreatedDate.Year == createdDate.Value.Year : true) &&
                    (applicantFullName != null ? r.ApplicantFullName.ToLower().Contains(applicantFullName.ToLower()) : true)
                ).ToListAsync();
            }
            else
            {
                return await _context.Applications.Select(r => new ApplicationDTO
                {
                    ApplicationNumber = "req-" + r.CreatedDate.Year.ToString() + "-" + r.Id,
                    Id = r.Id,
                    ApplicationStatusId = r.ApplicationStatusId,
                    ApplicantFullName = r.ApplicantFullName,
                    Subdivision = r.Subdivision,
                    ApplicationStatus = r.ApplicationStatus.Name,
                    TypeEquipmentId = r.TypeEquipmentId,
                    TypeEquipment = r.TypeEquipment.Name,
                    Description = r.Description,
                    CreatedDate = r.CreatedDate,
                    UpdateDate = r.UpdateDate,
                    HandlerComment = r.HandlerComment
                }).Where(r =>
                    (statusId != null ? r.ApplicationStatusId == statusId : true) &&
                    (typeEquipmentId != null ? r.TypeEquipmentId == typeEquipmentId : true) &&
                    (subdivision != null ? r.Subdivision.ToLower() == subdivision.ToLower() : true) &&
                    (createdDate != null ? r.CreatedDate.Day == createdDate.Value.Day && r.CreatedDate.Month == createdDate.Value.Month && r.CreatedDate.Year == createdDate.Value.Year : true) &&
                    (applicantFullName != null ? r.ApplicantFullName.ToLower().Contains(applicantFullName.ToLower()) : true)
                ).Skip((page - 1) * (int)pageSize)
            .Take((int)pageSize).ToListAsync();
            }
        }

        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDTO>> GetApplication(long id)
        {
          if (_context.Applications == null)
          {
              return NotFound();
          }
            var application = await _context.Applications.Select(r => new ApplicationDTO
            {
                ApplicationNumber = "req-" + r.CreatedDate.Year.ToString() + "-" + r.Id,
                Id = r.Id,
                ApplicationStatusId = r.ApplicationStatusId,
                ApplicantFullName = r.ApplicantFullName,
                Subdivision = r.Subdivision,
                ApplicationStatus = r.ApplicationStatus.Name,
                TypeEquipmentId = r.TypeEquipmentId,
                TypeEquipment = r.TypeEquipment.Name,
                Description = r.Description,
                CreatedDate = r.CreatedDate,
                UpdateDate = r.UpdateDate,
                HandlerComment = r.HandlerComment
            }).FirstOrDefaultAsync(r => r.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        // PUT: api/Applications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(long id, string applicantFullName, string subdivision, Int64 typeEquipmentId, string description)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            if (application.ApplicationStatusId == 3 || application.ApplicationStatusId == 4 || application.ApplicationStatusId == 5)
            {
                return NotFound(new { error = "InvalidStatusTransition", message = "Можно изменить заявку только без финального статуса" });
            }
            var typeEquipment = await _context.TypeEquipments.FindAsync(typeEquipmentId);
            if (typeEquipment == null)
            {
                return NotFound(new { error = "Error", message = "Указан неверный Id оборудования" });
            }
            else if (!typeEquipment.IsActive)
            {
                return NotFound(new { error = "Error", message = "Данное оборудование деактивировано" });
            }

                application.ApplicantFullName = applicantFullName;
            application.Subdivision = subdivision;
            application.TypeEquipmentId = typeEquipmentId;
            application.Description = description;
            application.UpdateDate = DateTime.Now;

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
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

        // POST: api/Applications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Application>> PostApplication(string applicantFullName, string subdivision, Int64 typeEquipmentId, string description)
        {
            var typeEquipment = await _context.TypeEquipments.FindAsync(typeEquipmentId);
            if (typeEquipment == null)
            {
                return NotFound(new { error = "Error", message = "Указан неверный Id оборудования" });
            }
            else if (!typeEquipment.IsActive)
            {
                return NotFound(new { error = "Error", message = "Данное оборудование деактивировано" });
            }
            Application application = new Application();
            application.ApplicantFullName = applicantFullName;
            application.Subdivision = subdivision;
            application.TypeEquipmentId = typeEquipmentId;
            application.Description = description;
            application.ApplicationStatusId = 1;
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplication", new { id = application.Id }, application);
        }

        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(long id)
        {
            if (_context.Applications == null)
            {
                return NotFound();
            }
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            if (application.ApplicationStatusId != 1)
            {

                return NotFound(new { error = "InvalidStatusTransition", message = "Можно удалить заявку только со статусом New" });
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationExists(long id)
        {
            return (_context.Applications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
