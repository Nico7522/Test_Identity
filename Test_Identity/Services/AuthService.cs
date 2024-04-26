using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Test_Identity.Models;

namespace Test_Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userMananger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _userMananger = userManager;
            _configuration = configuration;
            _contextAccessor = contextAccessor;

        }

        public string GenerateToken(LoginUser user)
        {


        IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.UserName),

                // All users admin let's go
                new Claim(ClaimTypes.Role, "Admin")

            };

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


        SecurityToken securityToken = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddHours(3),
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }

        public async Task<Claim?> GetRole(string userId)
        {
            ApplicationUser? user = await _userMananger.FindByIdAsync(userId);
            if(user is not null)
            {
                var claims = await _userMananger.GetClaimsAsync(user);
                foreach (var claim in claims)
                {
                    if (claim.Type == "Role")
                    {
                        return claim;
                    }
                }
            }

            return null;


        }

        public async Task<bool> Login(LoginUser user)
        {
            var identityUser = await _userMananger.FindByEmailAsync(user.UserName);
            if (identityUser is null)
            {
                return false;
            }

            return await _userMananger.CheckPasswordAsync(identityUser, user.Password);  
        }

        public async Task<bool> RegisterUser(LoginUser user)
        {
            var identityUser = new ApplicationUser { UserName = user.UserName, Email = user.UserName, IsActive = user.IsActive, Birthdate = user.Birthdate  };

            var result = await _userMananger.CreateAsync(identityUser, user.Password);
            return result.Succeeded;
        }

        public async Task<bool> SetRole(string userId, string role)
        {
            var user = await _userMananger.FindByIdAsync(userId);
            if (user is not null)
            {
                var isClaimAdded = await _userMananger.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                if (isClaimAdded.Succeeded) { return true; }
            }

            return false;
        }
    }
}
