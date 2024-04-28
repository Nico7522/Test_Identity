using Microsoft.AspNetCore.Identity;

namespace Test_Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }

        public DateTime Birthdate { get; set; }

        public string UserStatus { get; set; }
    }
}
