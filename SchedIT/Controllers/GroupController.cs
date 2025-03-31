using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyMvcApp.Controllers;

public class GroupController : Controller
{
            private readonly AppDbContext _context;

        public GroupController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _context.Groups.Include(t => t.Faculty).ToListAsync();
            return View(groups);
        }

        public IActionResult Create()
        {
            var viewModel = GetGroupFormViewModel(new Group());
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Group group)
        {
            if (ModelState.IsValid)
            {
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(GetGroupFormViewModel(group));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound();
            
            return View(GetGroupFormViewModel(group));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Group group)
        {
            if (id != group.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(group);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(GetGroupFormViewModel(group));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound();

            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound();

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private GroupFormViewModel GetGroupFormViewModel(Group group)
        {
            return new GroupFormViewModel
            {
                Group = group,
                FacultiesOptions = _context.Faculties.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList()
            };
        }
}