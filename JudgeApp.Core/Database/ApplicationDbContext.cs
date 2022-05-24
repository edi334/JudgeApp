using JudgeApp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JudgeApp.Core.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Judging> JudgingEntities { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}