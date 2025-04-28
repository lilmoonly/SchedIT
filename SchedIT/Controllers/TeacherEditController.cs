using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MyMvcApp.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
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

            var viewModel = new TeacherFormViewModel
            {
                Teacher = teacher,
                FacultiesOptions = await _context.Faculties.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TeacherFormViewModel viewModel)
        {
            if (id != viewModel.Teacher.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Teachers.Update(viewModel.Teacher);
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "Дані про викладача змінено.";
                return RedirectToAction("Index", "Teacher");
            }

            viewModel.FacultiesOptions = await _context.Faculties.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToListAsync();

            return View(viewModel);
        }

    }
}