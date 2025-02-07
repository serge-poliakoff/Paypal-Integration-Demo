﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServerWebApplicationAttempt.DataAccess;

#nullable disable

namespace ServerWebApplicationAttempt.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241109001832_UpgradeOnSides")]
    partial class UpgradeOnSides
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ServerWebApplicationAttempt.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("LastMove")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)")
                        .HasDefaultValue("-1x-1b")
                        .HasColumnName("LastMove");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)")
                        .HasDefaultValue("waiting")
                        .HasColumnName("Status");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("games");
                });

            modelBuilder.Entity("ServerWebApplicationAttempt.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("name");

                    b.Property<string>("pass")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("pass");

                    b.HasKey("Id");

                    b.ToTable("players");
                });

            modelBuilder.Entity("ServerWebApplicationAttempt.Models.Side", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)")
                        .HasColumnName("Color");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Date")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int?>("EnemyId")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)")
                        .HasColumnName("Result");

                    b.HasKey("Id");

                    b.HasIndex("EnemyId")
                        .IsUnique()
                        .HasFilter("[EnemyId] IS NOT NULL");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("sides");
                });

            modelBuilder.Entity("ServerWebApplicationAttempt.Models.Game", b =>
                {
                    b.HasOne("ServerWebApplicationAttempt.Models.Player", null)
                        .WithMany("Games")
                        .HasForeignKey("PlayerId");
                });

            modelBuilder.Entity("ServerWebApplicationAttempt.Models.Side", b =>
                {
                    b.HasOne("ServerWebApplicationAttempt.Models.Side", "Enemy")
                        .WithOne()
                        .HasForeignKey("ServerWebApplicationAttempt.Models.Side", "EnemyId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("ServerWebApplicationAttempt.Models.Game", "Game")
                        .WithMany("Sides")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ServerWebApplicationAttempt.Models.Player", "Player")
                        .WithMany("Sides")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Enemy");

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("ServerWebApplicationAttempt.Models.Game", b =>
                {
                    b.Navigation("Sides");
                });

            modelBuilder.Entity("ServerWebApplicationAttempt.Models.Player", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Sides");
                });
#pragma warning restore 612, 618
        }
    }
}
