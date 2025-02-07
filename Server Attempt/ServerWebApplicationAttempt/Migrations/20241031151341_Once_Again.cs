using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWebApplicationAttempt.Migrations
{
    /// <inheritdoc />
    public partial class Once_Again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "pass",
                table: "players",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false, defaultValue: "waiting"),
                    LastMove = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false, defaultValue: "-1x-1b"),
                    PlayerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_games_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "sides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sides_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_sides_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_games_PlayerId",
                table: "games",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_sides_GameId",
                table: "sides",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_sides_PlayerId",
                table: "sides",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sides");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.AlterColumn<string>(
                name: "pass",
                table: "players",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
