using JudgeApp.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace JudgeApp.Core.Services.Abstractions;

public interface IIdentityService
{
    Task<ActionResponse> Login(LoginRequest request);
    Task<ActionResponse<string>> Register(RegisterRequest request, string role);
}