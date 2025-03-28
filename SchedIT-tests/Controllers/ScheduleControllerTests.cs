using Microsoft.AspNetCore.Mvc.Rendering;
using MyMvcApp.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace ScheduleControllerTests
{
    public class ScheduleControllerTests
    {
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
    }
}
