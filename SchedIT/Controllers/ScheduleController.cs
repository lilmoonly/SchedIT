using Microsoft.AspNetCore.Mvc;
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
            
            var schedule = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.TimeEntry)
                .ToListAsync();

            var scheduleView = schedule
                .Select(s => $"{s.TimeEntry?.Value}: {s.Subject?.Name}")
                .ToList();


            return View(scheduleView);
        }
    }
}
