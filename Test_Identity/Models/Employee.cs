using Microsoft.AspNetCore.Identity;

namespace Test_Identity.Models
{
    public class Employee : IdentityUser<Guid>
    {
        public Guid EmployeeId { get; set; }

        public string Name { get; set; }

        public string Family { get; set; }

        public Employee()
        {
            
        }

        public Employee(string userName) : base(userName) { }
            
    }
}
