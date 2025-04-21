using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace MyMvcApp.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class FacultyController : Controller
    {
        private readonly AppDbContext _context;

        public FacultyController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var faculties = await _context.Faculties.ToListAsync();
            return View(faculties);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _context.Faculties.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(faculty);
        }
    }
}
