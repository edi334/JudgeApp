using AutoMapper;
using JudgeApp.Api.DTOs;
using JudgeApp.Core.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JudgeApp.API.ApiControllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IMapper _mapper;


    public ProjectController(IProjectRepository projectRepository, IMapper mapper, IStatusRepository statusRepository)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _statusRepository = statusRepository;
    }

    [HttpGet("{userId}")]
    [Authorize(Roles = "Judge", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> GetAll([FromRoute] string userId)
    {
        var projects = await _projectRepository.GetProjects(userId);
        var response = _mapper.Map<List<ProjectDto>>(projects.Item);

        return Ok(response);
    }

    [HttpGet("user/{id}")]
    [Authorize(Roles = "Participant", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> GetProjectByUserId([FromRoute] Guid id)
    {
        var project = await _projectRepository.GetProjectByUserId(id);
        if (project.HasErrors())
        {
            return BadRequest(project.Errors);
        }

        var response = _mapper.Map<ProjectDto>(project.Item);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Participant", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Create([FromBody] ProjectDto projectDto)
    {
        var status = await _statusRepository.GetActiveStatus();
        if (status.Item.Name != "Project Upload")
        {
            return BadRequest("You can't create project now");
        }

        var project = await _projectRepository.CreateProject(projectDto.Name, projectDto.Description,
            projectDto.VideoLink,
            projectDto.GithubLink, projectDto.UserId.ToString());
        if (project.HasErrors())
        {
            return BadRequest(project.Errors);
        }

        var response = _mapper.Map<ProjectDto>(project.Item);
        return Ok(response);
    }

    [HttpPatch]
    [Authorize(Roles = "Participant", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Edit([FromBody] ProjectDto projectDto)
    {
        var status = await _statusRepository.GetActiveStatus();
        if (status.Item.Name != "Project Upload")
        {
            return BadRequest("You can't create project now");
        }

        var project = await _projectRepository.EditProject(projectDto.Id, projectDto.Name, projectDto.Description,
            projectDto.VideoLink,
            projectDto.GithubLink);
        if (project.HasErrors())
        {
            return BadRequest(project.Errors);
        }

        var response = _mapper.Map<ProjectDto>(project.Item);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Participant", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _projectRepository.DeleteProject(id);
        if (response.HasErrors())
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Item);
    }
}