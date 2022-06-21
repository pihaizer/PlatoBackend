using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class Competitors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CompetitionId = table.Column<long>(type: "bigint", nullable: false),
                    Group = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "Competitions",
                newName: "pictureUrl");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Competitions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
