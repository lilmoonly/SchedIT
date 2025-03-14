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
            var schedule = await _context.Schedules.ToListAsync();
            
            var scheduleView = schedule.Select(s => $"{s.Time}: {s.Subject}").ToList();

            return View(scheduleView);
        }
    }
}
