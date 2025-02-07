using System;
using System.Collections.Generic;
using EF_TestApp1_aka_GangsterManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_TestApp1_aka_GangsterManager.Data;

public partial class TestGangstasDatabaseContext : DbContext
{
    public TestGangstasDatabaseContext()
    {
    }

    public TestGangstasDatabaseContext(DbContextOptions<TestGangstasDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Gang> Gangs { get; set; }

    public virtual DbSet<Gangster> Gangsters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NATE_HIGGERS\\SQLEXPRESS;Initial Catalog=TestGangstasDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gang>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.GangName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Gang_Name");
        });

        modelBuilder.Entity<Gangster>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Gang).WithMany(p => p.Gangsters)
                .HasForeignKey(d => d.GangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gangsters_Gangs");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
