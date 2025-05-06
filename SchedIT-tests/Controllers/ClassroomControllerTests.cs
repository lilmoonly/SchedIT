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

namespace ClassroomControllerTests
{
    public class ClassroomControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new AppDbContext(options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            dbContext.Classrooms.AddRange(new List<Classroom>
            {
            new Classroom { Id = 1, Number = "111", Building = "Головний корпус", Capacity = 30},
            new Classroom { Id = 2, Number = "261", Building = "Головний корпус", Capacity = 70 }
            });

            dbContext.SaveChanges();
            return dbContext;
        }

        // This test verifies that the Create() GET action returns a ViewResult to display the form.
        [Fact]
        public void TestCreate_Get_ReturnsView()
        {
            var dbContext = GetDbContext();
            var controller = new ClassroomController(dbContext);

            var result = controller.Create() as ViewResult;

            Assert.NotNull(result);
        }

    }
}
