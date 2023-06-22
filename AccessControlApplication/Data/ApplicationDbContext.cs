using AccessControlApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessControlApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        
        } 
        public DbSet<Register> UserDetails { get; set; }
    }
}
