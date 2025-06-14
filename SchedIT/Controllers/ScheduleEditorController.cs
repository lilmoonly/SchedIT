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

        public IActionResult Index(string sortOrder, string dayFilter, string subjectFilter, string teacherFilter, string classroomFilter)
        {
            // Сортування
            ViewData["DaySortParm"] = sortOrder == "Day" ? "day_desc" : "Day";
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TimeSortParm"] = sortOrder == "Time" ? "time_desc" : "Time";
            ViewData["SubjectSortParm"] = sortOrder == "Subject" ? "subject_desc" : "Subject";
            ViewData["TeacherSortParm"] = sortOrder == "Teacher" ? "teacher_desc" : "Teacher";
            ViewData["ClassroomSortParm"] = sortOrder == "Classroom" ? "classroom_desc" : "Classroom";
            
            var days = _context.Days.ToList();
            ViewData["DayFilter"] = new SelectList(days, "Id", "Value", dayFilter);

            // Фільтри для предметів, викладачів, аудиторій
            ViewData["SubjectFilterList"] = _context.Subjects.Select(s => s.Name).ToList();
            ViewData["TeacherFilterList"] = _context.Teachers.Select(t => t.FullName).ToList();
            ViewData["ClassroomFilterList"] = _context.Classrooms.Select(c => c.Number.ToString()).ToList();

            // Початковий запит на отримання розкладів
            var schedules = _context.Schedules
                .Include(s => s.DayEntry)
                .Include(s => s.Subject)
                .Include(s => s.TimeEntry)
                .Include(s => s.Teacher)
                .Include(s => s.Classroom)
                .AsQueryable();

            // Фільтрація за днем
            if (!string.IsNullOrEmpty(dayFilter))
            {
                schedules = schedules.Where(s => s.DayEntry.Id == int.Parse(dayFilter));
            }

            // Фільтрація за предметом
            if (!string.IsNullOrEmpty(subjectFilter))
            {
                schedules = schedules.Where(s => s.Subject.Name == subjectFilter);
            }

            // Фільтрація за викладачем
            if (!string.IsNullOrEmpty(teacherFilter))
            {
                schedules = schedules.Where(s => s.Teacher.FullName == teacherFilter);
            }

            // Фільтрація за аудиторією
            if (!string.IsNullOrEmpty(classroomFilter))
            {
                schedules = schedules.Where(s => s.Classroom.Number.ToString() == classroomFilter);
            }

            // Порядок днів
            var dayOrder = new Dictionary<string, int>
            {
                { "Понеділок", 1 },
                { "Вівторок", 2 },
                { "Середа", 3 },
                { "Четвер", 4 },
                { "Пʼятниця", 5 },
                { "Субота", 6 },
                { "Неділя", 7 }
            };

            // Сортування
            switch (sortOrder)
            {
                case "day_desc":
                    schedules = schedules
                        .AsEnumerable()
                        .OrderByDescending(s => dayOrder.GetValueOrDefault(s.DayEntry.Value ?? "", 99))
                        .AsQueryable();
                    break;
                case "Day":
                    schedules = schedules
                        .AsEnumerable()
                        .OrderBy(s => dayOrder.GetValueOrDefault(s.DayEntry.Value ?? "", 99))
                        .AsQueryable();
                    break;
                case "Time":
                    schedules = schedules.OrderBy(s => s.TimeEntry.Value);
                    break;
                case "time_desc":
                    schedules = schedules.OrderByDescending(s => s.TimeEntry.Value);
                    break;
                case "Subject":
                    schedules = schedules.OrderBy(s => s.Subject.Name);
                    break;
                case "subject_desc":
                    schedules = schedules.OrderByDescending(s => s.Subject.Name);
                    break;
                case "Teacher":
                    schedules = schedules.OrderBy(s => s.Teacher.FullName);
                    break;
                case "teacher_desc":
                    schedules = schedules.OrderByDescending(s => s.Teacher.FullName);
                    break;
                case "Classroom":
                    schedules = schedules.OrderBy(s => s.Classroom.Number);
                    break;
                case "classroom_desc":
                    schedules = schedules.OrderByDescending(s => s.Classroom.Number);
                    break;
                default:
                    schedules = schedules.OrderBy(s => s.DayEntry.Value);
                    break;
            }

            return View(schedules.ToList());
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
                }).ToList(),
            };
        }
    }
}
