using JudgeApp.Core.Entities;
using JudgeApp.Core.Utils;

namespace JudgeApp.Core.Services.Abstractions;

public interface IJudgingRepository
{
    Task<ActionResponse<List<Judging>>> AddOrUpdate(List<Judging> judgingEntities);
}