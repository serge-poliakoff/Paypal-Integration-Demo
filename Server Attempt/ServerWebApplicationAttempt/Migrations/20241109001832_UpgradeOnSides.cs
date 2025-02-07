using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWebApplicationAttempt.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeOnSides : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "sides",
                type: "Date",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "EnemyId",
                table: "sides",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_sides_EnemyId",
                table: "sides",
                column: "EnemyId",
                unique: true,
                filter: "[EnemyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_sides_sides_EnemyId",
                table: "sides",
                column: "EnemyId",
                principalTable: "sides",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sides_sides_EnemyId",
                table: "sides");

            migrationBuilder.DropIndex(
                name: "IX_sides_EnemyId",
                table: "sides");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "sides");

            migrationBuilder.DropColumn(
                name: "EnemyId",
                table: "sides");
        }
    }
}
