using Microsoft.EntityFrameworkCore;
using WebTest.Models;

namespace WebTest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }

        public DbSet <Student> Students { get; set; }
        public DbSet <Department> Departments { get; set; }
    }
}
