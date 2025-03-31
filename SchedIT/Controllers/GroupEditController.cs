using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyMvcApp.Controllers;

public class GroupEditController : Controller
{
    private readonly AppDbContext _context;

    public GroupEditController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Edit(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        var viewModel = new GroupFormViewModel
        {
            Group = group,
            FacultiesOptions = await _context.Faculties.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToListAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, GroupFormViewModel viewModel)
    {
        if (id != viewModel.Group.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _context.Groups.Update(viewModel.Group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Group");
        }

        viewModel.FacultiesOptions = await _context.Faculties.Select(f => new SelectListItem
        {
            Value = f.Id.ToString(),
            Text = f.Name
        }).ToListAsync();

        return View(viewModel);
    }
}