using System.ComponentModel.DataAnnotations.Schema;

namespace JudgeApp.Core.Entities;

public class Judging
{
    public Guid Id { get; set; }
    
    [ForeignKey("Judge")]
    public string JudgeId { get; set; }
    public ApplicationUser Judge { get; set; }
    
    [ForeignKey("Project")]
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    
    public int Standing { get; set; }
}