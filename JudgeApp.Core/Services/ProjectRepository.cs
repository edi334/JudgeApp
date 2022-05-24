using JudgeApp.Core.Database;
using JudgeApp.Core.Entities;
using JudgeApp.Core.Services.Abstractions;
using JudgeApp.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace JudgeApp.Core.Services;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<ActionResponse<List<Project>>> GetProjects()
    {
        var response = new ActionResponse<List<Project>>();
        var projects = await _dbContext.Projects.ToListAsync();
        response.Item = projects;

        return response;
    }

    public async Task<ActionResponse<Project>> GetProjectByUserId(Guid id)
    {
        var response = new ActionResponse<Project>();
        var projectByUserId = await _dbContext.Projects.FirstOrDefaultAsync(s => s.UserId == id.ToString());
        if (projectByUserId is null)
        {
            response.AddError("Project doesn't exists");
            return response;
        }

        response.Item = projectByUserId;

        return response;
    }

    public async Task<ActionResponse<Project>> CreateProject(string name, string description, string videoLink,
        string githubLink,
        string userId)
    {
        var response = new ActionResponse<Project>();

        var project = new Project
        {
            Name = name, Description = description, VideoLink = videoLink, GithubLink = githubLink, FinalStanding = 0,
            UserId = userId
        };
        var dbProject = await _dbContext.Projects.AddAsync(project);
        await _dbContext.SaveChangesAsync();
        response.Item = dbProject.Entity;
        return response;
    }

    public async Task<ActionResponse<Project>> EditProject(Guid id, string name, string description, string videoLink,
        string githubLink)
    {
        var response = new ActionResponse<Project>();
        var projectToChange = await _dbContext.Projects.FirstOrDefaultAsync(s => s.Id == id);

        if (projectToChange is null)
        {
            response.AddError("Project doesn't exists");
            return response;
        }

        projectToChange.Name = name;
        projectToChange.Description = description;
        projectToChange.VideoLink = videoLink;
        projectToChange.GithubLink = githubLink;

        await _dbContext.SaveChangesAsync();
        response.Item = projectToChange;
        return response;
    }

    public async Task<ActionResponse<Project>> DeleteProject(Guid id)
    {
        var response = new ActionResponse<Project>();
        var projectToDelete = await _dbContext.Projects.FirstOrDefaultAsync(s => s.Id == id);
        if (projectToDelete is null)
        {
            response.AddError("Project doesn't exists");
            return response;
        }

        _dbContext.Projects.Remove(projectToDelete);
        await _dbContext.SaveChangesAsync();
        response.Item = projectToDelete;
        return response;
    }
}