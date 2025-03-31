using System.ComponentModel.DataAnnotations;
using MyMvcApp.Models;
using Xunit;

namespace MyMvcApp.Tests.Models
{
    public class GroupTests
    {
        [Fact]
        public void Create_ValidGroup_Success()
        {
            var group = new Group
            {
                Id = 1,
                Name = "Software Engineering",
                FacultyId = 10
            };

            Assert.Equal(1, group.Id);
            Assert.Equal("Software Engineering", group.Name);
            Assert.Equal(10, group.FacultyId);
        }

        [Fact]
        public void Create_GroupWithNullName_ShouldFailValidation()
        {
            var group = new Group { FacultyId = 10 };
            var validationContext = new ValidationContext(group);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(group, validationContext, validationResults, true);

            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(Group.Name)));
        }

        [Fact]
        public void Create_GroupWithFaculty_Success()
        {
            var faculty = new Faculty { Id = 5, Name = "Faculty of Science" };
            var group = new Group { Id = 1, Name = "Physics", FacultyId = 5, Faculty = faculty };

            Assert.Equal(1, group.Id);
            Assert.Equal("Physics", group.Name);
            Assert.Equal(5, group.FacultyId);
            Assert.NotNull(group.Faculty);
            Assert.Equal("Faculty of Science", group.Faculty.Name);
        }
    }
}
