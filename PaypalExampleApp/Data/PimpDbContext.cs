using Microsoft.EntityFrameworkCore;
using PaypalExampleApp.Models;

namespace PaypalExampleApp.DbContexts;

public class PimpDbContext : DbContext
{
    public DbSet<PuteModel> putes { get; set; }

    public PimpDbContext()
    {

    }

    public PimpDbContext(DbContextOptions<PimpDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PuteModel>(entity =>
        {
            entity.Property<int>("Id").IsRequired().ValueGeneratedOnAdd();
            entity.Property("Name").IsRequired().HasMaxLength(30);
            entity.Property("Description").HasMaxLength(200);
            entity.Property("PriceHour").IsRequired().HasDefaultValue(149);
        });
    }
}