using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWebApplicationAttempt.Migrations
{
    /// <inheritdoc />
    public partial class CreatingSomeBugsToFIx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "sides");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "sides",
                type: "Date",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }
    }
}
