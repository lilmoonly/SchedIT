using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyMvcApp.Models
{
    public class TeacherFormViewModel
    {
        public Teacher Teacher { get; set; }

        public List<SelectListItem> FacultiesOptions { get; set; } = new();
    }
}