using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Controllers;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
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

            context.Faculties.Add(new Faculty { Id = 1, Name = "Факультет Інформатики" });
            context.Teachers.Add(new Teacher { Id = 1, FullName = "Коваль Наталія", FacultyId = 1 });
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task Edit_Get_ReturnsView_WhenTeacherExists()
        {
            var context = GetDbContext();
            var controller = new TeacherEditController(context);

            var result = await controller.Edit(1) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<TeacherFormViewModel>(result.Model);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenTeacherDoesNotExist()
        {
            var context = GetDbContext();
            var controller = new TeacherEditController(context);

            var result = await controller.Edit(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var context = GetDbContext();
            var controller = new TeacherEditController(context);

            var viewModel = new TeacherFormViewModel
            {
                Teacher = new Teacher { Id = 2, FullName = "Невірний ID" }
            };

            var result = await controller.Edit(1, viewModel);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ReturnsView_WhenModelStateIsInvalid()
        {
            var context = GetDbContext();
            var controller = new TeacherEditController(context);

            controller.ModelState.AddModelError("Teacher.FullName", "Required");

            var viewModel = new TeacherFormViewModel
            {
                Teacher = new Teacher { Id = 1 }
            };

            var result = await controller.Edit(1, viewModel) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<TeacherFormViewModel>(result.Model);
        }
    }
}
