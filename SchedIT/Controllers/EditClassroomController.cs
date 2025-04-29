using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace MyMvcApp.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class EditClassroomController : Controller
    {
        private readonly AppDbContext _context;

        public EditClassroomController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }
            return View(classroom);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Classroom classroom)
        {
            if (id != classroom.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existing = await _context.Classrooms.FindAsync(id);
                if (existing == null)
                {
                    return NotFound();
                }

                // Update only necessary properties
                existing.Number = classroom.Number;
                existing.Building = classroom.Building;
                existing.Capacity = classroom.Capacity;
                existing.Equipment = classroom.Equipment;

                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "Дані про аудиторію змінено.";
                return RedirectToAction("Index", "Classroom");
            }

            return View(classroom);
        }

    }
}
