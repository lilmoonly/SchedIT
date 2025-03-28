using MyMvcApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ClassroomTests
{
    public class ClassroomTests
    {
        [Fact]
        public void TestClassroom_ValidModel()
        {
            var classroom = new Classroom
            {
                Number = "111",
                Building = "Головний корпус",
                Capacity = 80,
                Equipment = "Проєктор, дошка"
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(classroom, new ValidationContext(classroom), validationResults, true);

            Assert.True(isValid);
        }

        [Fact]
        public void TestClassroom_InvalidCapacity()
        {
            var classroom = new Classroom
            {
                Number = "111",
                Building = "Головний корпус",
                Capacity = 0,
                Equipment = "Проєктор"
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(classroom, new ValidationContext(classroom), validationResults, true);

            Assert.False(isValid);
            Assert.Equal("Capacity must be at least 1", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void TestClassroom_MissingRequiredFields()
        {
            var classroom = new Classroom();

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(classroom, new ValidationContext(classroom), validationResults, true);

            Assert.False(isValid);
            Assert.Equal(3, validationResults.Count);
        }
    }
}
