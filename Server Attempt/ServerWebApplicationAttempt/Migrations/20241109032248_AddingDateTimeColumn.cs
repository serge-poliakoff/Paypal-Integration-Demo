using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWebApplicationAttempt.Migrations
{
    /// <inheritdoc />
    public partial class AddingDateTimeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "sides",
                type: "DATETIME",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "sides");
        }
    }
}
