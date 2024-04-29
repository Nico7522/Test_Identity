using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Test_Identity.Models;

namespace Test_Identity.Helpers.Jwt
{
    public class JwtFactory : IJwtFactory
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public JwtFactory(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {

            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            SecurityToken securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(3),
                    issuer: _configuration.GetSection("Jwt:Issuer").Value,
                    audience: _configuration.GetSection("Jwt:Audience").Value,
                    signingCredentials: signingCred
                    );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }

        public IList<Claim> SetBaseClaims(ApplicationUser user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Email, user.UserName),
                // What the user is and can tell which pages it can access.
                new Claim("UserStatus", "User"),
                // Role serves only to restrict access to some pages.
                new Claim(ClaimTypes.Role, "User"),
                new Claim("FidelityPoint", "800")

            };
        }
    }
}
