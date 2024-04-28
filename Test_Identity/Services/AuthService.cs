using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Test_Identity.Helpers;
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

        public async Task<string> GenerateToken(ApplicationUser user)
        {

            var claims = await _userMananger.GetClaimsAsync(user);

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

        public async Task<ApplicationUser?> Login(LoginUser user)
        {
            var identityUser = await _userMananger.FindByEmailAsync(user.UserName);
            if (identityUser is null)
            {
                return null;
            }

            if(await _userMananger.CheckPasswordAsync(identityUser, user.Password)) return identityUser;  

            return null;
        }

        public async Task<bool> RegisterUser(LoginUser user)
        {
            var applicationUser = new ApplicationUser { UserName = user.UserName, Email = user.UserName, IsActive = user.IsActive, Birthdate = user.Birthdate, UserStatus = "User"  };
            var userCreatedResult = await _userMananger.CreateAsync(applicationUser, user.Password);
            if (userCreatedResult.Succeeded) {
                var newUser = await _userMananger.FindByEmailAsync(user.UserName);
                if(newUser is not null)
                {
                    IList<Claim> userBaseClaims =  Helpers.Helpers.SetBaseClaims(newUser);
                    var resultClaims = await _userMananger.AddClaimsAsync(newUser, userBaseClaims);
                    return resultClaims.Succeeded;
                }
                return false;
            }

            return false;
        }

        public async Task<bool> SetRole(string userId, string role)
        {
            // Get user
            var user = await _userMananger.FindByIdAsync(userId);
            if (user is not null)
            {
                // Get "UserStatus" claim
                var oldClaim = await _userMananger.GetClaimsAsync(user).ContinueWith(r => r.Result.FirstOrDefault(c => c.Type == "UserStatus"));
                if (oldClaim is not null)
                {
                // Replace the old claim by the new claim.
                var modifiedClaim = await _userMananger.ReplaceClaimAsync(user, oldClaim, new Claim("UserStatus", role));
                if (modifiedClaim.Succeeded) { return true; }
                }

                // Return false if "UserStatus" claim has not be found. Normaly it never happen.
                return false;
            }

            // Return false if no user has been found.
            return false;
        }
    }
}
