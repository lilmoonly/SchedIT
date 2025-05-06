using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var db = services.GetRequiredService<AppDbContext>();

        // Безпечне виконання міграцій
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "SuperAdmin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Створення користувачів
        async Task CreateUserIfNotExists(string email, string password, string fullName, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser { UserName = email, Email = email, FullName = fullName };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        await CreateUserIfNotExists("admin@example.com", "Admin123!", "Admin", "Admin");
        await CreateUserIfNotExists("user@example.com", "User123!", "User", "User");
        await CreateUserIfNotExists("super@example.com", "Super123!", "Super Admin", "SuperAdmin");

        // Ініціалізація таблиць
        if (!db.Subjects.Any())
        {
            db.Subjects.AddRange(
                new Subject { Name = "Математика" },
                new Subject { Name = "Фізика" },
                new Subject { Name = "Програмування" }
            );
            db.SaveChanges();
        }

        if (!db.Times.Any())
        {
            db.Times.AddRange(
                new TimeEntry { Value = "08:30 - 09:50" },
                new TimeEntry { Value = "10:10 - 11:30" },
                new TimeEntry { Value = "11:50 - 13:10" },
                new TimeEntry { Value = "13:30 - 14:50" },
                new TimeEntry { Value = "15:05 - 16:25" },
                new TimeEntry { Value = "16:40 - 18:00" },
                new TimeEntry { Value = "18:10 - 19:30" }
            );
            db.SaveChanges();
        }

        if (!db.Days.Any())
        {
            db.Days.AddRange(
                new DayEntry { Value = "Понеділок" },
                new DayEntry { Value = "Вівторок" },
                new DayEntry { Value = "Середа" },
                new DayEntry { Value = "Четвер" },
                new DayEntry { Value = "П'ятниця" }
            );
            db.SaveChanges();
        }

        if (!db.Faculties.Any())
        {
            db.Faculties.AddRange(
                new Faculty { Name = "Факультет прикладної математики та інформатики" },
                new Faculty { Name = "Механіко-математичний факультет" },
                new Faculty { Name = "Факультет електроніки та комп’ютерних технологій" }
            );
            db.SaveChanges();
        }

        var fac1Id = db.Faculties.FirstOrDefault(t => t.Name == "Факультет прикладної математики та інформатики")?.Id;

        if (!db.Teachers.Any())
        {
            db.Teachers.Add(new Teacher
            {
                FullName = "Ляшко Олексій Володимирович",
                Position = "Доцент",
                FacultyId = fac1Id.Value
            });
            db.SaveChanges();
        }

        if (!db.Groups.Any())
        {
            db.Groups.Add(new Group
            {
                Name = "ПМІ-31",
                FacultyId = fac1Id.Value
            });
            db.SaveChanges();
        }

        if (!db.Classrooms.Any())
        {
            db.Classrooms.Add(new Classroom
            {
                Number = "101",
                Building = "Головний корпус",
                Capacity = 30,
                Equipment = "Проєктор"
            });
            db.SaveChanges();
        }

        var teacherId = db.Teachers.First().Id;
        var classroomId = db.Classrooms.First().Id;
        var subjectMathId = db.Subjects.First(s => s.Name == "Математика").Id;
        var subjectPhysId = db.Subjects.First(s => s.Name == "Фізика").Id;
        var subjectProgId = db.Subjects.First(s => s.Name == "Програмування").Id;
        var time1Id = db.Times.FirstOrDefault(t => t.Value == "08:30 - 09:50")?.Id;
        var time2Id = db.Times.FirstOrDefault(t => t.Value == "10:10 - 11:30")?.Id;
        var time3Id = db.Times.FirstOrDefault(t => t.Value == "11:50 - 13:10")?.Id;
        var day1Id = db.Days.FirstOrDefault(t => t.Value == "Понеділок")?.Id;
        var day2Id = db.Days.FirstOrDefault(t => t.Value == "Вівторок")?.Id;

        if (time1Id != null && time2Id != null && time3Id != null && day1Id != null && day2Id != null)
        {
            if (!db.Schedules.Any())
            {
                db.Schedules.AddRange(
                    new Schedule
                    {
                        DayEntryId = day1Id.Value,
                        TimeEntryId = time1Id.Value,
                        SubjectId = subjectMathId,
                        TeacherId = teacherId,
                        ClassroomId = classroomId
                    },
                    new Schedule
                    {
                        DayEntryId = day1Id.Value,
                        TimeEntryId = time2Id.Value,
                        SubjectId = subjectPhysId,
                        TeacherId = teacherId,
                        ClassroomId = classroomId
                    },
                    new Schedule
                    {
                        DayEntryId = day2Id.Value,
                        TimeEntryId = time3Id.Value,
                        SubjectId = subjectProgId,
                        TeacherId = teacherId,
                        ClassroomId = classroomId
                    }
                );
                db.SaveChanges();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Failed during app startup: {ex.Message}");
        
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
