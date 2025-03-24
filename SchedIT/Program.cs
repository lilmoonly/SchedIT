using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=schedule.db"));

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // Додати Subjects
    if (!db.Subjects.Any())
    {
        db.Subjects.AddRange(
            new Subject { Name = "Математика" },
            new Subject { Name = "Фізика" },
            new Subject { Name = "Програмування" }
        );
        db.SaveChanges();
    }

    // Додати TimeEntries
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

    // Додати викладача
    if (!db.Teachers.Any())
    {
        db.Teachers.Add(new Teacher
        {
            FullName = "Олексій Ляшко",
            Position = "Доцент",
            Faculty = "ФКН"
        });
        db.SaveChanges();
    }

    // Додати аудиторію
    if (!db.Classrooms.Any())
    {
        db.Classrooms.Add(new Classroom
        {
            Number = "101",
            Building = "Головний корпус",
            Capacity = 30,
            Equipment = "Проектор"
        });
        db.SaveChanges();
    }

    // Отримати потрібні Id
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

    // Перевірка на null перед додаванням розкладу
    if (time1Id != null && time2Id != null && time3Id != null && day1Id != null && day2Id != null)
    {
        // Додати розклад
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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
