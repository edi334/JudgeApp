using JudgeApp.Core.Entities;
using JudgeApp.Core.Utils;

namespace JudgeApp.Core.Services.Abstractions;

public interface IProjectRepository
{
    Task<ActionResponse<List<Project>>> GetProjects(string userId);
    Task<ActionResponse<List<Project>>> GetAll();
    Task<ActionResponse<Project>> GetProjectByUserId(Guid id);

    Task<ActionResponse<Project>> CreateProject(string name, string description, string videoLink, string githubLink,
        string userId);

    Task<ActionResponse<Project>> EditProject(Guid id, string name, string description, string videoLink,
        string githubLink);

    Task<ActionResponse<Project>> DeleteProject(Guid id);
    Task SaveProjects();
}