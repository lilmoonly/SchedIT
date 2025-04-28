using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace MyMvcApp.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Teachers.Include(t => t.Faculty).ToListAsync();
            return View(teachers);
        }

        public IActionResult Create()
        {
            var viewModel = GetTeacherFormViewModel(new Teacher());
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "Дані про викладача додано.";
                return RedirectToAction("Index");
            }
            return View(GetTeacherFormViewModel(teacher));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();
            
            return View(GetTeacherFormViewModel(teacher));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Teacher teacher)
        {
            if (id != teacher.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(GetTeacherFormViewModel(teacher));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Дані про викладача видалено.";
            return RedirectToAction("Index");
        }

        private TeacherFormViewModel GetTeacherFormViewModel(Teacher teacher)
        {
            return new TeacherFormViewModel
            {
                Teacher = teacher,
                FacultiesOptions = _context.Faculties.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList()
            };
        }
    }
}