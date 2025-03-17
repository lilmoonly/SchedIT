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

    if (!db.Schedules.Any())
    {
        db.Schedules.AddRange(
            new Schedule { Time = "08:00 - 09:30", Subject = "Математика" },
            new Schedule { Time = "09:45 - 11:15", Subject = "Фізика" },
            new Schedule { Time = "11:30 - 13:00", Subject = "Програмування" }
        );
        db.SaveChanges();
    }

    if (!db.Classrooms.Any())
    {
        var classroom = new Classroom
        {
            Number = "101",
            Building = "Головний корпус",
            Capacity = 30,
            Equipment = "Проектор"
        };
        db.Classrooms.Add(classroom);
        db.SaveChanges();
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
