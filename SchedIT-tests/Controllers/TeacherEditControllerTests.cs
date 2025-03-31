using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Controllers;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Threading.Tasks;
using Xunit;

namespace TeacherEditControllerTests
{
    public class TeacherEditControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDb_{System.Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Teachers.Add(new Teacher { Id = 1, FullName = "Коваль Наталія" });
            context.SaveChanges();

            return context;
        }

        // This test checks that GET Edit returns a View if the teacher exists
        [Fact]
        public async Task Edit_Get_ReturnsView_WhenTeacherExists()
        {
            var context = GetDbContext();
            var controller = new TeacherEditController(context);

            var result = await controller.Edit(1) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<Teacher>(result.Model);
        }

        // This test checks that GET Edit returns NotFound if teacher does not exist
        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenTeacherDoesNotExist()
        {
            var context = GetDbContext();
            var controller = new TeacherEditController(context);

            var result = await controller.Edit(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // This test checks that POST Edit returns BadRequest if id does not match teacher.ID
        [Fact]
        public async Task Edit_Post_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var context = GetDbContext();
            var controller = new TeacherEditController(context);

            var teacher = new Teacher { Id = 2, FullName = "Невірний ID" };

            var result = await controller.Edit(1, teacher);

            Assert.IsType<BadRequestResult>(result);
        }

        // This test checks that POST Edit successfully updates teacher and redirects if ModelState is valid
        [Fact]
        public async Task Edit_Post_UpdatesTeacher_WhenModelStateIsValid()
        {
            var context = GetDbContext();
            var controller = new TeacherEditController(context);

            var teacher = new Teacher { Id = 1, FullName = "Коваль Наталія Оновлена" };

            var result = await controller.Edit(1, teacher) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Teacher", result.ControllerName);

            var updated = await context.Teachers.FindAsync(1);
            Assert.Equal("Коваль Наталія Оновлена", updated.FullName);
        }

        // This test checks that POST Edit returns View if ModelState is invalid
        [Fact]
        public async Task Edit_Post_ReturnsView_WhenModelStateIsInvalid()
        {
            var context = GetDbContext();
            var controller = new TeacherEditController(context);

            controller.ModelState.AddModelError("FullName", "Required");

            var teacher = new Teacher { Id = 1 };

            var result = await controller.Edit(1, teacher) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<Teacher>(result.Model);
        }
    }
}
