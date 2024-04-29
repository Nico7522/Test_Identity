using System.Security.Claims;
using Test_Identity.Models;

namespace Test_Identity.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(LoginUser user);
        Task<string?> Login(LoginUser user);
        Task<bool> SetRole(string userId, string role);
        Task<Claim?> GetRole(string userId);
    }
}