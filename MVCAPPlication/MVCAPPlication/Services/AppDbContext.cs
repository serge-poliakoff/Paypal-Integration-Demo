using Microsoft.EntityFrameworkCore;
using MVCAPPlication.Models;

namespace MVCAPPlication.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
