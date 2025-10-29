using Demo_Entertainment_Company_Backend_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo_Entertainment_Company_Backend_API
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
