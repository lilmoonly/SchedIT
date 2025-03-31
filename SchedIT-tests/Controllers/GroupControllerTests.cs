using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Controllers;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GroupControllerTests
{
    public class GroupControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new AppDbContext(options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            dbContext.Faculties.Add(new Faculty { Id = 1, Name = "Faculty of Engineering" });
            dbContext.Groups.Add(new Group { Id = 1, Name = "Group A", FacultyId = 1 });
            dbContext.SaveChanges();

            return dbContext;
        }

        [Fact]
        public async Task Index_ReturnsViewWithGroups()
        {
            var dbContext = GetDbContext();
            var controller = new GroupController(dbContext);

            var result = await controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<List<Group>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Create_ValidGroup_RedirectsToIndex()
        {
            var dbContext = GetDbContext();
            var controller = new GroupController(dbContext);
            var newGroup = new Group { Id = 2, Name = "Group B", FacultyId = 1 };

            var result = await controller.Create(newGroup) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal(2, dbContext.Groups.Count());
        }

        [Fact]
        public async Task Edit_ValidId_ReturnsViewWithGroup()
        {
            var dbContext = GetDbContext();
            var controller = new GroupController(dbContext);

            var result = await controller.Edit(1) as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<GroupFormViewModel>(result.Model);
            Assert.Equal(1, model.Group.Id);
        }

        [Fact]
        public async Task Edit_InvalidId_ReturnsNotFound()
        {
            var dbContext = GetDbContext();
            var controller = new GroupController(dbContext);

            var result = await controller.Edit(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ValidId_RedirectsToIndex()
        {
            var dbContext = GetDbContext();
            var controller = new GroupController(dbContext);

            var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(dbContext.Groups);
        }
    }
}
