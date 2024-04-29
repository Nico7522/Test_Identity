using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Test_Identity.Helpers.Jwt;
using Test_Identity.Models;

namespace Test_Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        public AuthService(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
        }

        public async Task<string?> Login(LoginUser user)
        {
            // Try to find the user by his email.
            var identityUser = await _userManager.FindByEmailAsync(user.UserName);
            if (identityUser is null)
            {
                // If user has not be found, return null.
                return null;
            }

            // If user has been found check his password.
            if (await _userManager.CheckPasswordAsync(identityUser, user.Password))
            {
                // If the password match generate the token.
                string token = await _jwtFactory.GenerateToken(identityUser);
                return token;
            }
            // Otherwise, return null.
            return null;
        }

        public async Task<bool> RegisterUser(LoginUser user)
        {
            // Create the new ApplicationUser
            var applicationUser = new ApplicationUser { UserName = user.UserName, Email = user.UserName, IsActive = user.IsActive, Birthdate = user.Birthdate, UserStatus = "User"  };
            var userCreatedResult = await _userManager.CreateAsync(applicationUser, user.Password);
            // If user has been created
            if (userCreatedResult.Succeeded) {
                // Try to retrieve it 
                var newUser = await _userManager.FindByEmailAsync(user.UserName);

                // If user has been found
                if(newUser is not null)
                {
                    // Add the base claims.
                    IList<Claim> userBaseClaims =  _jwtFactory.SetBaseClaims(newUser);
                    var resultClaims = await _userManager.AddClaimsAsync(newUser, userBaseClaims);
                    return resultClaims.Succeeded;
                }
                // Otherwise return false.
                return false;
            }
            // Return false if the user has not been created.
            return false;
        }

        public async Task<bool> SetRole(string userId, string role)
        {
            // Get user
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                // Get "UserStatus" claim
                var oldClaim = await _userManager.GetClaimsAsync(user).ContinueWith(r => r.Result.FirstOrDefault(c => c.Type == "UserStatus"));
                if (oldClaim is not null)
                {
                // Replace the old claim by the new claim.
                var modifiedClaim = await _userManager.ReplaceClaimAsync(user, oldClaim, new Claim("UserStatus", role));
                if (modifiedClaim.Succeeded) { return true; }
                }

                // Return false if "UserStatus" claim has not be found. Normaly it never happen.
                return false;
            }

            // Return false if no user has been found.
            return false;
        }

        public async Task<Claim?> GetRole(string userId)
        {
            // Try to find the user
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            // If user has been found
            if (user is not null)
            {
                // Get his claims
                var claims = await _userManager.GetClaimsAsync(user);
                foreach (var claim in claims)
                {
                    // For all of his claims, if a claim type is "UserStatus", then return the claim.
                    if (claim.Type == "UserStatus")
                    {
                        return claim;
                    }
                }
            }
            // Return null if no user has been found.
            return null;
        }
    }
}
