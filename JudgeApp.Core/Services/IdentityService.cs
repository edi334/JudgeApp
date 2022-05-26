using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JudgeApp.Core.Entities;
using JudgeApp.Core.Services.Abstractions;
using JudgeApp.Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JudgeApp.Core.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<ActionResponse> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return new ActionResponse<string>
            {
                Action = "Login",
                Errors = new List<string> { "User does not exist!" }
            };
        }

        var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!validPassword)
        {
            return new ActionResponse<string>
            {
                Action = "Login",
                Errors = new List<string> { "Invalid credentials!" }
            };
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains("Admin"))
        {
            return new ActionResponse<string>
            {
                Action = "Login",
                Errors = new List<string> { "You can't login here with your Admin account!" }
            };
        }

        if (!roles.Contains("Judge") && !roles.Contains("Participant"))
        {
            return new ActionResponse<string>
            {
                Action = "Login",
                Errors = new List<string> { "You must be a Judge or Participant to login!" }
            };
        }

        var session = new Session
        {
            UserId = user.Id,
            TokenType = "Bearer",
            Token = GenerateToken(user, roles.FirstOrDefault()),
            Username = user.UserName,
            Role = roles.FirstOrDefault(),
        };

        return new ActionResponse<Session>
        {
            Action = "Login",
            Item = session
        };
    }

    public async Task<ActionResponse<string>> Register(RegisterRequest request, string role)
    {
        if (!request.Password.Equals(request.ConfirmPassword))
        {
            return new ActionResponse<string>
            {
                Action = "Register",
                Errors = new List<string> { "Passwords not matching!" }
            };
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true,
            Created = DateTime.Now,
            Updated = DateTime.Now
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            var response = new ActionResponse<string>
            {
                Action = "Register"
            };

            foreach (var error in createResult.Errors) response.AddError(error.Description);
            return response;
        }

        var addToRole = await _userManager.AddToRoleAsync(user, role);

        if (!addToRole.Succeeded)
        {
            var response = new ActionResponse<string>
            {
                Action = "Register"
            };

            foreach (var error in addToRole.Errors) response.AddError(error.Description);
            return response;
        }

        return new ActionResponse<string>
        {
            Action = "Register",
            Item = "User created successfully"
        };
    }

    private string GenerateToken(ApplicationUser newUser, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value);
        Console.WriteLine(_configuration.GetSection("JWT:Issuer").Value);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new(JwtRegisteredClaimNames.Sub, newUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.GetSection("JWT:Issuer").Value),
                new Claim("id", newUser.Id),
                new Claim(ClaimTypes.Role, role)
            }),
            Issuer = _configuration.GetSection("JWT:Issuer").Value,
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}