using System.Net.NetworkInformation;
using System.Security.Claims;
using Test_Identity.Models;

namespace Test_Identity.Helpers
{
    public static class Helpers
    {

        public static IList<Claim> SetBaseClaims(ApplicationUser user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim("UserStatus", "User")
        };
        }
    }
}
