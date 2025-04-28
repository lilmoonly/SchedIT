using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


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

            return View(schedules);
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
                existingRating.CreatedAt = DateTime.Now; 
            }
            else
            {
                var scheduleRating = new ScheduleRating
                {
                    UserId = userId,
                    Rating = rating,
                    ScheduleId = scheduleId,
                    CreatedAt = DateTime.Now
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
