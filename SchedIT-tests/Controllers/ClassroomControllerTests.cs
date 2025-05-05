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
        
        
        // This test ensures the Create(Classroom) POST action adds a new classroom,
        // redirects to the Index action, and the total count becomes three.
        // [Fact]
        // public void TestCreateClassroom()
        // {
        //     var dbContext = GetDbContext();
        //     var controller = new ClassroomController(dbContext);
        //     var newClassroom = new Classroom { Id = 3, Number = "116", Building = "Головний корпус", Capacity = 25 };

        //     var result = controller.Create(newClassroom) as RedirectToActionResult;

        //     Assert.NotNull(result);
        //     Assert.Equal("Index", result.ActionName);
        //     Assert.Equal(3, dbContext.Classrooms.Count());
        // }
        
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
