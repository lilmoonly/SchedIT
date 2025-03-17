using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;

namespace MyMvcApp.Controllers
{
    public class ScheduleEditorController : Controller
    {
        private readonly AppDbContext _context;

        public ScheduleEditorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var schedules = _context.Schedules.ToList();
            return View(schedules);
        }

        public IActionResult Edit(int id)
        {
            var schedule = _context.Schedules.Find(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        [HttpPost]
        public IActionResult Edit(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Schedules.Update(schedule);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(schedule);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Schedules.Add(schedule);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(schedule);
        }
    }
}
