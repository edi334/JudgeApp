using AutoMapper;
using JudgeApp.Api.DTOs;
using JudgeApp.Core.Entities;
using JudgeApp.Core.Services.Abstractions;
using JudgeApp.Core.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JudgeApp.API.ApiControllers;

[ApiController]
[Route("api/judging")]
[Authorize(Roles = "Judge", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class JudgingController : ControllerBase
{
    private readonly IJudgingRepository _judgingRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IMapper _mapper;

    public JudgingController(IJudgingRepository judgingRepository, IStatusRepository statusRepository, IMapper mapper)
    {
        _judgingRepository = judgingRepository;
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> AddOrUpdateJudging([FromBody] List<JudgingDto> judgingDtos)
    {
        var activeStatus = await _statusRepository.GetActiveStatus();

        if (activeStatus.Item.Name != "Judging")
        {
            return BadRequest("You can't judge now!");
        }

        var judgingEntities = _mapper.Map<List<Judging>>(judgingDtos);

        var judgingResponse = await _judgingRepository.AddOrUpdate(judgingEntities);

        if (judgingResponse.HasErrors())
        {
            return BadRequest(judgingResponse.GetErrors());
        }

        var response = _mapper.Map<JudgingDto>(judgingResponse.Item);

        return Ok(response);
    }
}