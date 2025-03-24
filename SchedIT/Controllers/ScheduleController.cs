using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyMvcApp.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var schedules = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.TimeEntry)
                .Include(s => s.Teacher)
                .Include(s => s.Classroom)
                .ToListAsync();

            return View(schedules);
        }
    }
}
