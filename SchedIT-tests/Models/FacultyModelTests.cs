using MyMvcApp.Models;
using System;
using Xunit;

namespace FacultyModelTests
{
    public class FacultyTests
    {
        [Theory]
        [InlineData("Faculty of Engineering", "FE")]
        [InlineData("Computer Science Department", "CSD")]
        [InlineData("Mathematics & Physics", "MP")]
        [InlineData("- Artificial Intelligence -", "AI")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void Name_Set_ShortNameIsGeneratedCorrectly(string inputName, string expectedShortName)
        {
            var faculty = new Faculty { Name = inputName };

            Assert.Equal(expectedShortName, faculty.ShortName);
        }
    }
}
