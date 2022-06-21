using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class CompetitorComplexKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Competitors",
                table: "Competitors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competitors",
                table: "Competitors",
                columns: new[] { "UserId", "CompetitionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Competitors",
                table: "Competitors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competitors",
                table: "Competitors",
                column: "UserId");
        }
    }
}
