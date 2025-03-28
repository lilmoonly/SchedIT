using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Controllers;
using MyMvcApp.Data;
using MyMvcApp.Models;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TeacherControllerTests
{
    public class TeacherControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new AppDbContext(options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            dbContext.Teachers.AddRange(new List<Teacher>
            {
            new Teacher { Id = 1, FullName = "Петренко Ігор Васильович" },
            new Teacher { Id = 2, FullName = "Коваль Наталі Степанівна" }
            });

            dbContext.SaveChanges();
            return dbContext;
        }

        [Fact]
        public async Task TestIndex_ReturnsViewWithTeachers()
        {
            var dbContext = GetDbContext();
            var controller = new TeacherController(dbContext);

            var result = await controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<List<Teacher>>(result.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task TestCreateTeacher()
        {
            var dbContext = GetDbContext();
            var controller = new TeacherController(dbContext);
            var newTeacher = new Teacher { Id = 3, FullName = "Вишневський Олег Васильович" };

            var result = await controller.Create(newTeacher) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal(3, dbContext.Teachers.Count());
        }

    }
}