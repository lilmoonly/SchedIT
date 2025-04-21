using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MyMvcApp.Controllers;

public class HomeController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View(new LoginViewModel()); 
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(user);

            // Редирект в залежності від ролі
            if (roles.Contains("Admin"))
                return RedirectToAction("Index", "Home");
            else if (roles.Contains("Manager"))
                return RedirectToAction("Index", "Home");
            else if (roles.Contains("Client"))
                return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Невірний логін або пароль");
        return View(model);
    }
}

