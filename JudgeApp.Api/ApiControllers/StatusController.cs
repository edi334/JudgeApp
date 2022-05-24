using AutoMapper;
using JudgeApp.Api.DTOs;
using JudgeApp.Core.Entities;
using JudgeApp.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace JudgeApp.API.ApiControllers;

[ApiController]
[Route("api/status")]
public class StatusController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IStatusRepository _statusRepository;

    public StatusController(IMapper mapper, IStatusRepository statusRepository)
    {
        _mapper = mapper;
        _statusRepository = statusRepository;
    }

    [HttpGet("active")]
    public async Task<ActionResult> GetActive()
    {
        var status = await _statusRepository.GetActiveStatus();
        if (status.HasErrors()) return NotFound();
        var response = _mapper.Map<StatusDto>(status.Item);
        return Ok(response);
    }
}