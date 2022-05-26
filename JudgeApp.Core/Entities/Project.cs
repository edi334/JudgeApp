using System.ComponentModel.DataAnnotations.Schema;

namespace JudgeApp.Core.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string VideoLink { get; set; }
    public string GithubLink { get; set; }
    public int FinalStanding { get; set; }
    public int Count { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}