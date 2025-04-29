using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyMvcApp.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ScheduleEditorController : Controller
    {
        private readonly AppDbContext _context;

        public ScheduleEditorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var schedules = _context.Schedules
                .Include(s => s.DayEntry)
                .Include(s => s.Subject)
                .Include(s => s.TimeEntry)
                .Include(s => s.Teacher)
                .Include(s => s.Classroom)
                .ToList();

            return View(schedules);
        }


        public IActionResult Add()
        {
            var viewModel = GetScheduleFormViewModel(new Schedule());
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Add(ScheduleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Schedules.Add(model.Schedule);
                _context.SaveChanges();
                TempData["ToastMessage"] = "Пару успішно додано!";
                return RedirectToAction("Index");
            }

            return View(GetScheduleFormViewModel(model.Schedule));
        }

        public IActionResult Edit(int id)
        {
            var schedule = _context.Schedules.Find(id);
            if (schedule == null)
            {
                return NotFound();
            }

            var viewModel = GetScheduleFormViewModel(schedule);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(ScheduleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Schedules.Update(model.Schedule);
                _context.SaveChanges();
                TempData["ToastMessage"] = "Дані про пару успішно змінено.";
                return RedirectToAction("Index");
            }

            return View(GetScheduleFormViewModel(model.Schedule));
        }

        // Допоміжний метод
        private ScheduleFormViewModel GetScheduleFormViewModel(Schedule schedule)
        {
            return new ScheduleFormViewModel
            {
                Schedule = schedule,
                DayOptions = _context.Days.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Value
                }).ToList(),
                SubjectOptions = _context.Subjects.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList(),
                TimeOptions = _context.Times.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Value
                }).ToList(),
                TeacherOptions = _context.Teachers.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"{t.FullName} ({t.Position})"
                }).ToList(),
                ClassroomOptions = _context.Classrooms.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Number}, {c.Building} ({c.Capacity} місць)"
                }).ToList()
            };
        }
    }
}
