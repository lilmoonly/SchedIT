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
        public void TestCreate_Get_ReturnsView()
        {
            var dbContext = GetDbContext();
            var controller = new GroupController(dbContext);

            var result = controller.Create() as ViewResult;

            Assert.NotNull(result);
        }
        

    }
}
