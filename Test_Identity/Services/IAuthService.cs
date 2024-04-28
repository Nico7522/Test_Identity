using System.Security.Claims;
using Test_Identity.Models;

namespace Test_Identity.Services
{
    public interface IAuthService
    {
        Task<string> GenerateToken(ApplicationUser user);
        Task<ApplicationUser?> Login(LoginUser user);
        Task<bool> RegisterUser(LoginUser user);
        Task<bool> SetRole(string userId, string role);
        Task<Claim?> GetRole(string userId);
    }
}