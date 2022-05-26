
using JudgeApp.Core.Database;
using JudgeApp.Core.Entities;
using JudgeApp.Core.Services.Abstractions;
using JudgeApp.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace JudgeApp.Core.Services;

public class JudgingRepository : IJudgingRepository
{
    private readonly ApplicationDbContext _dbContext;

    public JudgingRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ActionResponse<List<Judging>>> AddOrUpdate(List<Judging> judgingEntities)
    {
        var response = new ActionResponse<List<Judging>>();

        var projectsCount = await _dbContext.Projects.CountAsync();

        if (projectsCount != judgingEntities.Count)
        {
            response.AddError("Not all projects have been judged");
            return response;
        }
        
        foreach (var judging in judgingEntities)
        {
            var judgeExits = await _dbContext.Users.AnyAsync(u => u.Id == judging.JudgeId);

            if (!judgeExits)
            {
                response.AddError("Judge with this Id doesn't exist");
                return response;
            }

            var projectExists = await _dbContext.Projects.AnyAsync(p => p.Id == judging.ProjectId);

            if (!projectExists)
            {
                response.AddError("Project with this Id doesn't exist");
                return response;
            }

            var existingJudging = await _dbContext.JudgingEntities
                .FirstOrDefaultAsync(j => j.JudgeId == judging.JudgeId && j.ProjectId == judging.ProjectId);

            if (existingJudging is null)
            {
                await _dbContext.JudgingEntities.AddAsync(judging);
            }
            else
            {
                existingJudging.Standing = judging.Standing;
            }

            await _dbContext.SaveChangesAsync();
        }
        
        response.Item = judgingEntities;
        return response;
    }
}