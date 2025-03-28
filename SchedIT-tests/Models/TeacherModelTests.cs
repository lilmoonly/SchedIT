using MyMvcApp.Models;

namespace TeacherTests
{
    public class TeacherTests
    {
        [Theory]
        [InlineData("Асистент", "Асистент")]
        [InlineData("Професор", "Професор")]
        [InlineData("Доцент", "Доцент")]

        public void PositionIsCorrect(string position, string expected)
        {
            var teacher = new Teacher { Position = position };
            var result = teacher.Position;
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Марципан Марго Олегівна", "Марципан М. О.")]
        [InlineData("Малюга", "Малюга")]
        [InlineData(" ", "")]
        public void ShortNameIsCorrect(string fullName, string expected)
        {
            var teacher = new Teacher { FullName = fullName };
            var result = teacher.ShortName;

            Assert.Equal(expected, result);
        }
    }
}