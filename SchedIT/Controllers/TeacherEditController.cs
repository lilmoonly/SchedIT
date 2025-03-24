using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyMvcApp.Controllers
{
    public class TeacherEditController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherEditController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Teachers.Update(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Teacher");
            }

            return View(teacher);
        }
    }
}
