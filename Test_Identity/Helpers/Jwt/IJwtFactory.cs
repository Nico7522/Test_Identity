using System.Security.Claims;
using Test_Identity.Models;

namespace Test_Identity.Helpers.Jwt
{
    public interface IJwtFactory
    {
        Task<string> GenerateToken(ApplicationUser user);
        IList<Claim> SetBaseClaims(ApplicationUser user);

    }
}
