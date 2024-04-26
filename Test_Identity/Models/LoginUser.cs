namespace Test_Identity.Models
{
    public class LoginUser
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
