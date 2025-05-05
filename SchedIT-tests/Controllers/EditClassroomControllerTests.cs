using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Controllers;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Threading.Tasks;
using Xunit;

namespace EditClassroomControllerTests
{
    public class EditClassroomControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDb_{System.Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Classrooms.Add(new Classroom { Id = 1, Number = "101", Building = "Корпус A", Capacity = 30 });
            context.SaveChanges();

            return context;
        }

        // This test checks that GET Edit returns a View if the classroom exists
        [Fact]
        public async Task Edit_Get_ReturnsView_WhenClassroomExists()
        {
            var context = GetDbContext();
            var controller = new EditClassroomController(context);

            var result = await controller.Edit(1) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<Classroom>(result.Model);
        }

        // This test checks that GET Edit returns NotFound if the classroom does not exist
        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenClassroomDoesNotExist()
        {
            var context = GetDbContext();
            var controller = new EditClassroomController(context);

            var result = await controller.Edit(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // This test checks that POST Edit returns BadRequest if ids do not match
        [Fact]
        public async Task Edit_Post_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var context = GetDbContext();
            var controller = new EditClassroomController(context);

            var classroom = new Classroom { Id = 2, Number = "111", Building = "Корпус B", Capacity = 40 };

            var result = await controller.Edit(1, classroom);

            Assert.IsType<BadRequestResult>(result);
        }

        // This test checks that POST Edit updates classroom and redirects when ModelState is valid
        // [Fact]
        // public async Task Edit_Post_UpdatesClassroom_WhenModelStateIsValid()
        // {
        //     var context = GetDbContext();
        //     var controller = new EditClassroomController(context);

        //     var updatedClassroom = new Classroom { Id = 1, Number = "222", Building = "Корпус B", Capacity = 50 };

        //     var result = await controller.Edit(1, updatedClassroom) as RedirectToActionResult;

        //     Assert.NotNull(result);
        //     Assert.Equal("Index", result.ActionName);
        //     Assert.Equal("Classroom", result.ControllerName);

        //     var classroomInDb = await context.Classrooms.FindAsync(1);
        //     Assert.Equal("222", classroomInDb.Number);
        //     Assert.Equal(50, classroomInDb.Capacity);
        // }

        // This test checks that POST Edit returns View when ModelState is invalid
        [Fact]
        public async Task Edit_Post_ReturnsView_WhenModelStateIsInvalid()
        {
            var context = GetDbContext();
            var controller = new EditClassroomController(context);

            controller.ModelState.AddModelError("Number", "Required");

            var classroom = new Classroom { Id = 1 };

            var result = await controller.Edit(1, classroom) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<Classroom>(result.Model);
        }
    }
}
