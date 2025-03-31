using MyMvcApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace SubjectTests
{
    public class SubjectTests
    {
        [Fact]
        public void TestSubjectValidModel()
        {
            var subject = new Subject
            {
                Name = "Дискретна математика"
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(subject, new ValidationContext(subject), validationResults, true);

            Assert.True(isValid);
        }

        [Fact]
        public void TestSubjectMissingName()
        {
            var subject = new Subject();

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(subject, new ValidationContext(subject), validationResults, true);

            Assert.False(isValid);
            Assert.Single(validationResults);
        }
    }
}
