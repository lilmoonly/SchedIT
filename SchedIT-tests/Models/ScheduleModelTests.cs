using MyMvcApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ScheduleTests
{
    public class ScheduleTests
    {
        [Fact]
        public void TestSchedule_ValidModel()
        {
            var schedule = new Schedule
            {
                DayEntryId = 1,
                SubjectId = 1,
                TimeEntryId = 1,
                TeacherId = 1,
                ClassroomId = 1
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(schedule, new ValidationContext(schedule), validationResults, true);

            Assert.True(isValid);
        }

    }
}
