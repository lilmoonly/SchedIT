using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Controllers;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FacultyControllerTests
{
    public class FacultyControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        [Fact]
        public async Task Index_ReturnsViewWithFaculties()
        {
            var dbContext = GetDbContext();
            dbContext.Faculties.Add(new Faculty { Name = "Faculty of Engineering" });
            dbContext.SaveChanges();

            var controller = new FacultyController(dbContext);
            var result = await controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<List<Faculty>>(result.Model);
            Assert.Single(model);
        }
        
        [Fact]
        public void TestCreate_Get_ReturnsView()
        {
            var dbContext = GetDbContext();
            var controller = new FacultyController(dbContext);

            var result = controller.Create() as ViewResult;

            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task Create_ValidFaculty_RedirectsToIndex()
        {
            var dbContext = GetDbContext();
            var controller = new FacultyController(dbContext);
            var newFaculty = new Faculty { Name = "Faculty of Science" };

            var result = await controller.Create(newFaculty) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal(1, dbContext.Faculties.Count());
        }
    }
}