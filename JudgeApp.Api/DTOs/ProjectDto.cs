using Microsoft.Build.Framework;

namespace JudgeApp.Api.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }

    public string VideoLink { get; set; }

    public string GithubLink { get; set; }

    public int FinalStanding { get; set; }
    public Guid UserId { get; set; }
}