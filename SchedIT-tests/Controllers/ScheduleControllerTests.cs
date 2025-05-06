using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Controllers;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ScheduleEditorControllerTests
{
    public class ScheduleEditorControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestScheduleDb")
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            // Populate necessary related data
            var subject = new Subject { Id = 1, Name = "Math" };
            var teacher = new Teacher { Id = 1, FullName = "Іван Петренко", Position = "Доцент" };
            var classroom = new Classroom { Id = 1, Number = "101", Building = "Головний", Capacity = 40 };
            var time = new TimeEntry { Id = 1, Value = "08:30 - 10:00" };
            var day = new DayEntry { Id = 1, Value = "Понеділок" };

            dbContext.Subjects.Add(subject);
            dbContext.Teachers.Add(teacher);
            dbContext.Classrooms.Add(classroom);
            dbContext.Times.Add(time);
            dbContext.Days.Add(day);

            dbContext.Schedules.Add(new Schedule
            {
                Id = 1,
                SubjectId = subject.Id,
                TeacherId = teacher.Id,
                ClassroomId = classroom.Id,
                TimeEntryId = time.Id,
                DayEntryId = day.Id
            });

            dbContext.SaveChanges();
            return dbContext;
        }

        [Fact]
        public void TestIndex_ReturnsViewWithSchedules()
        {
            var dbContext = GetDbContext();
            var controller = new ScheduleEditorController(dbContext);

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<Schedule>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public void TestAdd_Get_ReturnsViewWithFormViewModel()
        {
            var dbContext = GetDbContext();
            var controller = new ScheduleEditorController(dbContext);

            var result = controller.Add() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ScheduleFormViewModel>(result.Model);
        }

        [Fact]
        public void TestAdd_Post_ValidModel_AddsScheduleAndRedirects()
        {
            var dbContext = GetDbContext();
            var controller = new ScheduleEditorController(dbContext);

            var model = new ScheduleFormViewModel
            {
                Schedule = new Schedule
                {
                    SubjectId = 1,
                    TeacherId = 1,
                    ClassroomId = 1,
                    DayEntryId = 1,
                    TimeEntryId = 1
                }
            };

            var result = controller.Add(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal(2, dbContext.Schedules.Count());
        }

        [Fact]
        public void TestAdd_Post_InvalidModel_ReturnsFormView()
        {
            var dbContext = GetDbContext();
            var controller = new ScheduleEditorController(dbContext);
            controller.ModelState.AddModelError("Error", "Fake error");

            var model = new ScheduleFormViewModel { Schedule = new Schedule() };

            var result = controller.Add(model) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ScheduleFormViewModel>(result.Model);
        }

        [Fact]
        public void TestEdit_Get_ExistingId_ReturnsFormView()
        {
            var dbContext = GetDbContext();
            var controller = new ScheduleEditorController(dbContext);

            var result = controller.Edit(1) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ScheduleFormViewModel>(result.Model);
        }

        [Fact]
        public void TestEdit_Get_NonExistingId_ReturnsNotFound()
        {
            var dbContext = GetDbContext();
            var controller = new ScheduleEditorController(dbContext);

            var result = controller.Edit(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void TestEdit_Post_ValidModel_UpdatesScheduleAndRedirects()
        {
            var dbContext = GetDbContext();
            var controller = new ScheduleEditorController(dbContext);

            var updatedSchedule = dbContext.Schedules.First();
            updatedSchedule.ClassroomId = 1;

            var model = new ScheduleFormViewModel
            {
                Schedule = updatedSchedule
            };

            var result = controller.Edit(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void TestEdit_Post_InvalidModel_ReturnsFormView()
        {
            var dbContext = GetDbContext();
            var controller = new ScheduleEditorController(dbContext);
            controller.ModelState.AddModelError("Error", "Fake error");

            var model = new ScheduleFormViewModel { Schedule = new Schedule() };

            var result = controller.Edit(model) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ScheduleFormViewModel>(result.Model);
        }
    }
}
