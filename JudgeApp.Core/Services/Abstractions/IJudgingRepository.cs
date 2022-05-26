using JudgeApp.Core.Entities;
using JudgeApp.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace JudgeApp.Core.Services.Abstractions;

public interface IJudgingRepository
{
    Task<ActionResponse<List<Judging>>> GetAll();
    Task<ActionResponse<List<Judging>>> AddOrUpdate(List<Judging> judgingEntities);
}