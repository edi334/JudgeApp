using AutoMapper;
using JudgeApp.Api.DTOs;
using JudgeApp.Core.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JudgeApp.API.ApiControllers;

[ApiController]
[Route("api/final-standings")]
[Authorize(Roles = "Participant,Judge", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FinalStandingsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IMapper _mapper;

    public FinalStandingsController(IProjectRepository projectRepository, IMapper mapper, IStatusRepository statusRepository)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _statusRepository = statusRepository;
    }

    [HttpGet]
    public async Task<ActionResult> GetFinalStandings()
    {
        var status = await _statusRepository.GetActiveStatus();

        if (status.Item.Name != "Getting Results")
        {
            return BadRequest("You can't generate final standings now!");
        }
        
        var projects = await _projectRepository.GetAll();
        var response = _mapper.Map<List<ProjectDto>>(projects.Item);

        return Ok(response);
    }
}