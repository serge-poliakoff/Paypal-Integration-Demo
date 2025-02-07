using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using ServerWebApplicationAttempt.Models;

namespace ServerWebApplicationAttempt.DataAccess
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Player> players { get; set; }
        public virtual DbSet<Side> sides { get; set; }
        public virtual DbSet<Game> games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.name)
                    .HasMaxLength(40)
                    .HasColumnName("name");
                entity.Property(e => e.pass)
                    .HasMaxLength(10)
                    .HasColumnName("pass");
            });

            modelBuilder.Entity<Side>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Color)
                    .HasMaxLength(5)
                    .HasColumnName("Color");
                entity.Property(e => e.Result)
                    .HasMaxLength(4)
                    .HasColumnName("Result");
                entity.Property(e => e.Date)
                    .HasColumnType("DATETIME");

                entity.HasOne(e => e.Player).WithMany(p => p.Sides)
                    .HasForeignKey(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Game).WithMany(g => g.Sides)
                    .HasForeignKey(e => e.GameId)
                    .OnDelete(DeleteBehavior.NoAction);     //game entities should be deleted, all info is in sides
                entity.HasOne(e => e.Enemy).WithOne()
                    .OnDelete(DeleteBehavior.NoAction);
                
            });
            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e =>e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Status)
                    .HasMaxLength(8)
                    .HasDefaultValue("waiting")
                    .HasColumnName("Status");
                entity.Property(e => e.LastMove)
                    .HasMaxLength(6)
                    .HasDefaultValue("-1x-1b")
                    .HasColumnName("LastMove");

            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
