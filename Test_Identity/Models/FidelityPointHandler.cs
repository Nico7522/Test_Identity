using Microsoft.AspNetCore.Authorization;

namespace Test_Identity.Models
{
    public class FidelityPointHandler : AuthorizationHandler<FidelityRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FidelityRequirement requirement)
        {
            // Complete task if the claim is not present.
            if (!context.User.HasClaim(c => c.Type == "FidelityPoint"))
            {
                return Task.CompletedTask;
            }

            // Retrieve the points
            int fidelityPoint = int.Parse(context.User.FindFirst(c => c.Type == "FidelityPoint")?.Value);

            // Success if points are >= to the required points
            if (fidelityPoint >= requirement.FidelityPoints)
            {
            context.Succeed(requirement);
            return Task.CompletedTask;

            }
            // Otherwise, fail.
            context?.Fail();
            return Task.CompletedTask;
        }
    }
}
