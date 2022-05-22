using JudgeApp.Core.Entities;
using JudgeApp.Core.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JudgeApp.API.Controllers;

[Authorize(Roles = "Admin")]
public class StatusController : Controller
{
    private readonly IStatusRepository _statusRepository;

    public StatusController(IStatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var statuses = await _statusRepository.GetStatuses();
        return View(statuses.Item);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Status status)
    {
        if (!ModelState.IsValid) return View();
        var response = await _statusRepository.CreateStatus(status.Name);
        if (response.HasErrors())
        {
            ModelState.AddModelError("", response.GetErrors());
            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var response = await _statusRepository.GetById(Guid.Parse(id));
        if (response.HasErrors())
        {
            return RedirectToAction(nameof(Index));
        }

        return View(response.Item);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Status status)
    {
        if (!ModelState.IsValid) return View();
        var response = await _statusRepository.EditStatus(status);
        if (response.HasErrors())
        {
            ModelState.AddModelError("", response.GetErrors());
            return View();
        }

        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _statusRepository.GetById(Guid.Parse(id));
        if (response.HasErrors())
        {
            return RedirectToAction(nameof(Index));
        }

        return View(response.Item);
    }
    
    [HttpPost]
    public async Task<IActionResult> ComfirmDelete(string id)
    {
        if (!ModelState.IsValid) return View("Delete");
        var response = await _statusRepository.DeleteStatus(Guid.Parse(id));
        if (response.HasErrors())
        {
            ModelState.AddModelError("", response.GetErrors());
            return View("Delete");
        }

        return RedirectToAction(nameof(Index));
    }
}