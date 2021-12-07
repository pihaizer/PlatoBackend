using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Routes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Value",
                table: "Tags",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sends_UserId_ClimbingRouteId",
                table: "Sends",
                columns: new[] { "UserId", "ClimbingRouteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId_ClimbingRouteId",
                table: "Likes",
                columns: new[] { "UserId", "ClimbingRouteId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Value",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Sends_UserId_ClimbingRouteId",
                table: "Sends");

            migrationBuilder.DropIndex(
                name: "IX_Likes_UserId_ClimbingRouteId",
                table: "Likes");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Routes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
