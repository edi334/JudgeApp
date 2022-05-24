namespace JudgeApp.Api.DTOs;

public class JudgingDto
{
    public string JudgeId { get; set; }
    public Guid ProjectId { get; set; }
    public int Standing { get; set; }
}