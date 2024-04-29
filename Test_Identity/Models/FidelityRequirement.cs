using Microsoft.AspNetCore.Authorization;

namespace Test_Identity.Models
{
    public class FidelityRequirement : IAuthorizationRequirement
    {
        public int FidelityPoints { get; set; }

        public FidelityRequirement(int fidelityPoint)
        {
            FidelityPoints = fidelityPoint;
        }
    }
}
