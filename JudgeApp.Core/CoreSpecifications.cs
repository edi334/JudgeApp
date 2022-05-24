using JudgeApp.Core.Services;
using JudgeApp.Core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeApp.Core;

public static class CoreSpecifications
{
    public static IServiceCollection AddCoreSpecifications(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        
        return services;
    }
}