using System;
using System.Collections.Generic;
using EF_Example2.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_Example2.Data;

public partial class ZernoContext : DbContext
{
    public ZernoContext()
    {
    }

    public ZernoContext(DbContextOptions<ZernoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NATE_HIGGERS\\SQLEXPRESS;Initial Catalog=Zerno;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EmploiId).HasColumnName("EmploiID");
            entity.Property(e => e.FullName)
                .HasMaxLength(25)
                .IsFixedLength();
            entity.Property(e => e.WorkPointId).HasColumnName("WorkPointID");

            entity.HasOne(d => d.Emploi).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmploiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Posts");

            entity.HasOne(d => d.WorkPoint).WithMany(p => p.Employees)
                .HasForeignKey(d => d.WorkPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Shops");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Schedule)
                .HasMaxLength(3)
                .IsFixedLength();
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Adress)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Materials).HasDefaultValue(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
