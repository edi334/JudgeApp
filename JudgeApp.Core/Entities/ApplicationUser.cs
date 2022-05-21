using Microsoft.AspNetCore.Identity;

namespace JudgeApp.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}