using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyMvcApp.Models;

public class GroupFormViewModel
{
    public Group Group { get; set; }

    public List<SelectListItem> FacultiesOptions { get; set; } = new();
}