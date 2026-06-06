using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.DataModels;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalsController : ControllerBase
    {
        private readonly TestContext _context;

        public ApprovalsController(TestContext context)
        {
            _context = context;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(long id, long applicationId, string? handlerComment = null)
        {
            var applicationStatus = await _context.ApplicationStatuses.FindAsync(id);
            if (applicationStatus == null)
            {
                return NotFound(new { error = "Error", message = "Указан неверный Id статуса" });
            }
            var application = await _context.Applications.FindAsync(applicationId);
            if (application == null)
            {
                return NotFound(new { error = "Error", message = "Указан неверный Id заявки" });
            }
            var typeEquipment = await _context.TypeEquipments.FindAsync(application.TypeEquipmentId);
            if (!typeEquipment.IsActive)
            {
                return NotFound(new { error = "Error", message = "Неактивный тип оборудования" });
            }
            if (application.ApplicationStatusId == 1 && (id == 2 || id == 5))
            {
                application.ApplicationStatusId = id;
                if (id == 5)
                {
                    if (handlerComment != null)
                    {
                        application.HandlerComment = handlerComment;
                    }
                    else
                    {
                        return NotFound(new { error = "Error", message = "отклонение заявки только с комментарием" });
                    }
                }

                application.UpdateDate = DateTime.Now;
                _context.Entry(application).State = EntityState.Modified;
            }
            else if (application.ApplicationStatusId == 1)
            {
                return NotFound(new { error = "Error", message = "Заявку можно перевести из New только в InProgress,Cancelled" });
            }
            else
            if (application.ApplicationStatusId == 2 && (id == 3 || id == 4 || id == 5))
            {
                application.ApplicationStatusId = id;
                if (id == 5)
                {
                    if (handlerComment != null)
                    {
                        application.HandlerComment = handlerComment;
                    }
                    else
                    {
                        return NotFound(new { error = "Error", message = "отклонение заявки только с комментарием" });
                    }
                }

                application.UpdateDate = DateTime.Now;
                _context.Entry(application).State = EntityState.Modified;
            }
            else if (application.ApplicationStatusId == 2)
            {
                return NotFound(new { error = "Error", message = "Заявку можно перевести из InProgress  только в Approved,Rejected,Cancelled" });
            }
            else
            if (application.ApplicationStatusId == 3 || application.ApplicationStatusId == 4 || application.ApplicationStatusId == 5)
            {
                return NotFound(new { error = "Error", message = "Заявку нельзя изменить после перехода в финальный статус Approved,Rejected,Cancelled" });
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                return NotFound(new { error = "Error", message = ex.Message });
            }
            return NoContent();
        }
    }
}
