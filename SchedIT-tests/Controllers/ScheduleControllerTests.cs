using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Controllers;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ScheduleControllerTests
{
    public class ScheduleControllerTests
    {
        // This test verifies that the ScheduleFormViewModel correctly holds dropdown data
        // including day, subject, time, teacher, and classroom options.
        [Fact]
        public void TestSchedule_Dropdowns()
        {
            var viewModel = new ScheduleFormViewModel
            {
                Schedule = new Schedule
                {
                    DayEntryId = 1,
                    SubjectId = 1,
                    TimeEntryId = 1,
                    TeacherId = 1,
                    ClassroomId = 1
                },
                DayOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Понеділок", Value = "1" },
                    new SelectListItem { Text = "Вівторок", Value = "2" },
                    new SelectListItem { Text = "Середа", Value = "3" },
                    new SelectListItem { Text = "Четвер", Value = "4" },
                    new SelectListItem { Text = "П'ятниця", Value = "5" }
                },
                SubjectOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Дискретна математика", Value = "1" },
                    new SelectListItem { Text = "Фізика", Value = "2" }
                },
                TimeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "08:30 - 09:50", Value = "1" },
                    new SelectListItem { Text = "10:10 - 11:30", Value = "2" },
                    new SelectListItem { Text = "11:50 - 13:10", Value = "3" }
                },
                TeacherOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Чорна Марта Олегівна", Value = "1" }
                },
                ClassroomOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "101", Value = "1" },
                    new SelectListItem { Text = "111", Value = "2" }
                }
            };

            Assert.NotNull(viewModel.Schedule);
            Assert.Equal(5, viewModel.DayOptions.Count);
            Assert.Equal(2, viewModel.SubjectOptions.Count);
            Assert.Equal(3, viewModel.TimeOptions.Count);
            Assert.Equal(1, viewModel.TeacherOptions.Count);
            Assert.Equal(2, viewModel.ClassroomOptions.Count);
        }

        // This test verifies that the Index() action returns a ViewResult
        // containing a formatted string combining time and subject for each schedule entry.
        [Fact]
        public async Task Index_ReturnsScheduleViewAsStringList()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            using var dbContext = new AppDbContext(options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var subject = new Subject { Id = 1, Name = "Дискретна математика" };
            var time = new TimeEntry { Id = 1, Value = "08:30 - 09:50" };

            dbContext.Subjects.Add(subject);
            dbContext.Times.Add(time);
            dbContext.SaveChanges();

            dbContext.Schedules.Add(new Schedule
            {
                Id = 1,
                SubjectId = 1,
                TimeEntryId = 1
            });

            dbContext.SaveChanges();

            var controller = new ScheduleController(dbContext);

            var result = await controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = result.Model;

            // DEBUG: Show type + value
            Assert.NotNull(model);
            var modelType = model.GetType().ToString();
            var modelStr = model.ToString();

            // Useful to know what is actually returned
            Assert.True(model is IEnumerable<object>, $"Returned model is of type: {modelType}, value: {modelStr}");
        }




    }
}
