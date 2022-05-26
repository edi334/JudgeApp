using JudgeApp.Core.Services.Abstractions;
using JudgeApp.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JudgeApp.API.Controllers;

[Authorize(Roles = "Admin")]
public class FinalStandingsController : Controller
{
    private readonly IProjectRepository _projectRepository;
    private readonly IJudgingRepository _judgingRepository;

    public FinalStandingsController(IJudgingRepository judgingRepository, IProjectRepository projectRepository)
    {
        _judgingRepository = judgingRepository;
        _projectRepository = projectRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GenerateFinalStandings()
    {
        var projects = await _projectRepository.GetAll();

        foreach (var project in projects.Item)
        {
            var judgingResult = await _judgingRepository.GetAll();
            var judgingEntities = judgingResult.Item.Where(j => j.ProjectId == project.Id).ToList();

            project.Count = 0;

            foreach (var judging in judgingEntities)
            {
                var points = PointsMapper.GetPoints(judging.Standing);
                project.Count += points;
            }
        }

        int index = 1;
        var orderedProjects = projects.Item.OrderByDescending(p => p.Count);
        foreach (var project in orderedProjects)
        {
            project.FinalStanding = index;
            index++;
            project.Count = 0;
        }

        await _projectRepository.SaveProjects();

        return RedirectToAction(nameof(Index));
    }
}