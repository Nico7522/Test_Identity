using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Test_Identity.Models;

namespace Test_Identity.Context
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<ApplicationUser>().Property(u => u.IsActive).HasDefaultValue(true);
        //}
        public DbSet<Employee> Employee { get; set; }
    }
}
