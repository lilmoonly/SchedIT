using Microsoft.EntityFrameworkCore;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace MyMvcApp.Data
{
    public class AppDbContext :IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TimeEntry> Times { get; set; }
        public DbSet<DayEntry> Days { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ScheduleRating> ScheduleRatings { get; set; }

    }
}
