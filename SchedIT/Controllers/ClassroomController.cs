using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace MyMvcApp.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ClassroomController : Controller
    {
        private readonly AppDbContext _context;

        public ClassroomController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var classrooms = _context.Classrooms.ToList();
            return View(classrooms);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                _context.Classrooms.Add(classroom);
                TempData["ToastMessage"] = "Аудиторію успішно додано!.";
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classroom);
        }
    }
}
