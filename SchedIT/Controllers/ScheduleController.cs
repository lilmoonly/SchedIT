using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;

namespace MyMvcApp.Controllers
{
    [Authorize(Roles = "User, Admin, SuperAdmin")]
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScheduleController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var schedules = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.TimeEntry)
                .Include(s => s.DayEntry)
                .Include(s => s.Teacher)
                .Include(s => s.Classroom)
                .ToListAsync();

            var daysOrder = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "Понеділок", 1 }, { "Вівторок", 2 }, { "Середа", 3 }, { "Четвер", 4 },
                { "П’ятниця", 5 }, { "Субота", 6 }, { "Неділя", 7 }
            };

            var filteredSchedule = schedules
                .Where(s => !string.IsNullOrWhiteSpace(s.DayEntry?.Value) && !string.IsNullOrWhiteSpace(s.TimeEntry?.Value));

            var orderedSchedule = filteredSchedule
                .OrderBy(s => daysOrder.TryGetValue(s.DayEntry.Value.Trim(), out int dayOrder) ? dayOrder : int.MaxValue)
                .ThenBy(s =>
                {
                    var parts = s.TimeEntry.Value.Split('-');
                    var startTimeText = parts.Length > 0 ? parts[0].Trim() : s.TimeEntry.Value.Trim();
                    return TimeSpan.TryParse(startTimeText, out TimeSpan time) ? time : TimeSpan.MaxValue;
                })
                .ToList();

            var groupedSchedule = orderedSchedule
                .GroupBy(s => s.DayEntry.Value.Trim())
                .OrderBy(g => daysOrder.TryGetValue(g.Key, out int dayOrder) ? dayOrder : int.MaxValue);

            // Перевірка на конфлікти
            var conflicts = new List<Schedule>();
            foreach (var schedule in orderedSchedule)
            {
                // Заміна ScheduleId на Id
                var conflictForTeacher = orderedSchedule.Any(s => s.Id != schedule.Id && 
                                                                s.TimeEntry.Value == schedule.TimeEntry.Value &&
                                                                s.DayEntry.Value == schedule.DayEntry.Value &&
                                                                s.TeacherId == schedule.TeacherId);

                var conflictForClassroom = orderedSchedule.Any(s => s.Id != schedule.Id && 
                                                                    s.TimeEntry.Value == schedule.TimeEntry.Value &&
                                                                    s.DayEntry.Value == schedule.DayEntry.Value &&
                                                                    s.ClassroomId == schedule.ClassroomId);


                if (conflictForTeacher || conflictForClassroom)
                {
                    conflicts.Add(schedule);
                    Console.WriteLine($"Conflict detected for Schedule ID: {schedule.Id}, Day: {schedule.DayEntry.Value}, Time: {schedule.TimeEntry.Value}");
                }
            }

            // Передаємо конфлікти в вигляд
            ViewBag.Conflicts = conflicts;
            ViewBag.DaysOrder = daysOrder;

            return View(groupedSchedule);
        }

        [HttpPost]
        public async Task<IActionResult> RateSchedule(int rating, int scheduleId)
        {
            if (rating < 0 || rating > 5)
            {
                ModelState.AddModelError("", "Оцінка повинна бути від 0 до 5.");
                return RedirectToAction("Index");
            }
            
            var userId = _userManager.GetUserId(User);
            
            var existingRating = _context.ScheduleRatings
                .FirstOrDefault(r => r.UserId == userId && r.ScheduleId == scheduleId);

            if (existingRating != null)
            {
                existingRating.Rating = rating;
                existingRating.CreatedAt = DateTime.UtcNow; 
            }
            else
            {
                var scheduleRating = new ScheduleRating
                {
                    UserId = userId,
                    Rating = rating,
                    ScheduleId = scheduleId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.ScheduleRatings.Add(scheduleRating);
            }

            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Вашу оцінку відправлено.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> ViewRatings()
        {
            var ratings = _context.ScheduleRatings
                .Include(r => r.User) 
                .ToList();
            
            var ratingsWithEmail = ratings.Select(r => new 
            {
                r.Rating,
                r.CreatedAt,
                UserEmail = r.User.Email 
            }).ToList();

            return View(ratingsWithEmail);
        }
    }
}
