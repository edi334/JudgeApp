using JudgeApp.Core.Database;
using JudgeApp.Core.Entities;
using JudgeApp.Core.Services.Abstractions;
using JudgeApp.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace JudgeApp.Core.Services;

public class StatusRepository : IStatusRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StatusRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ActionResponse<List<Status>>> GetStatuses()
    {
        var response = new ActionResponse<List<Status>>();
        var statuses = await _dbContext.Statuses.ToListAsync();
        response.Item = statuses;

        return response;
    }

    public async Task<ActionResponse<Status>> GetActiveStatus()
    {
        var response = new ActionResponse<Status>();
        var activeStatus = await _dbContext.Statuses.FirstOrDefaultAsync(s => s.Active == true);

        if (activeStatus is null)
        {
            response.AddError("No active status found");
            return response;
        }

        response.Item = activeStatus;
        return response;
    }

    public async Task<ActionResponse<Status>> CreateStatus(string name)
    {
        var response = new ActionResponse<Status>();
        var nameExists = await _dbContext.Statuses.AnyAsync(s => s.Name == name);
        if (nameExists)
        {
            response.AddError("Status with this name already exists");
            return response;
        }

        var status = new Status {Name = name, Active = false};
        var dbStatus = await _dbContext.Statuses.AddAsync(status);
        await _dbContext.SaveChangesAsync();
        response.Item = dbStatus.Entity;
        return response;
    }

    public async Task<ActionResponse<Status>> EditStatus(Status status)
    {
        var response = new ActionResponse<Status>();
        var statusToChange = await _dbContext.Statuses.FirstOrDefaultAsync(s => s.Id == status.Id);
        if (statusToChange is null)
        {
            response.AddError("Status doesn't exists");
            return response;
        }
        
        statusToChange.Name = status.Name;
        if (status.Active)
        {
            var statuses = await _dbContext.Statuses.ToListAsync();
            foreach (var s in statuses)
            {
                s.Active = false;
            }
        }
        statusToChange.Active = status.Active;
        await _dbContext.SaveChangesAsync();
        response.Item = status;
        return response;
    }

    public async Task<ActionResponse<Status>> DeleteStatus(Guid id)
    {
        var response = new ActionResponse<Status>();
        var statusToDelete = await _dbContext.Statuses.FirstOrDefaultAsync(s => s.Id == id);
        if (statusToDelete is null)
        {
            response.AddError("Status doesn't exists");
            return response;
        }

        _dbContext.Statuses.Remove(statusToDelete);
        await _dbContext.SaveChangesAsync();
        response.Item = statusToDelete;
        return response;
    }

    public async Task<ActionResponse<Status>> GetById(Guid id)
    {
        var response = new ActionResponse<Status>();
        var status = await _dbContext.Statuses.FirstOrDefaultAsync(s => s.Id == id);

        if (status is null)
        {
            response.AddError("Status doesn't exists");
            return response;
        }

        response.Item = status;
        return response;
    }
}