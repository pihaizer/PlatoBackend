using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class UserUniqueKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_FirebaseId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirebaseId",
                table: "Users",
                column: "FirebaseId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_FirebaseId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirebaseId",
                table: "Users",
                column: "FirebaseId");
        }
    }
}
