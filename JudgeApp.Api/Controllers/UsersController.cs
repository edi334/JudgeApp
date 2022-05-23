using JudgeApp.Core.Entities;
using JudgeApp.Core.Services.Abstractions;
using JudgeApp.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JudgeApp.API.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly IIdentityService _identityService;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(IIdentityService identityService, UserManager<ApplicationUser> userManager)
    {
        _identityService = identityService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.GetUsersInRoleAsync("Judge");
        return View(users);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RegisterRequest model)
    {
        if (!ModelState.IsValid) return View();

        var response = await _identityService.Register(model, "Judge");

        if (response.HasErrors())
        {
            foreach (var error in response.Errors) ModelState.AddModelError("", error);
            return View();
        }

        return RedirectToAction(nameof(Index));
    }
}