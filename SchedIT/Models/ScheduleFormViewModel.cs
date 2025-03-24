using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyMvcApp.Models
{
    public class ScheduleFormViewModel
    {
        public Schedule Schedule { get; set; }

        public List<SelectListItem> SubjectOptions { get; set; } = new();
        public List<SelectListItem> TimeOptions { get; set; } = new();
        public List<SelectListItem> TeacherOptions { get; set; } = new();
        public List<SelectListItem> ClassroomOptions { get; set; } = new();
    }
}