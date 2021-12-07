using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    public partial class MakeSetterString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_ClimbingRouteModel_ModelId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Setter_SetterId",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "Setter");

            migrationBuilder.DropIndex(
                name: "IX_Routes_SetterId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClimbingRouteModel",
                table: "ClimbingRouteModel");

            migrationBuilder.DropColumn(
                name: "SetterId",
                table: "Routes");

            migrationBuilder.RenameTable(
                name: "ClimbingRouteModel",
                newName: "RouteModels");

            migrationBuilder.AddColumn<string>(
                name: "Setter",
                table: "Routes",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RouteModels",
                table: "RouteModels",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClimbingRouteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookmarks_Routes_ClimbingRouteId",
                        column: x => x.ClimbingRouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_ClimbingRouteId",
                table: "Bookmarks",
                column: "ClimbingRouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_RouteModels_ModelId",
                table: "Routes",
                column: "ModelId",
                principalTable: "RouteModels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_RouteModels_ModelId",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RouteModels",
                table: "RouteModels");

            migrationBuilder.DropColumn(
                name: "Setter",
                table: "Routes");

            migrationBuilder.RenameTable(
                name: "RouteModels",
                newName: "ClimbingRouteModel");

            migrationBuilder.AddColumn<long>(
                name: "SetterId",
                table: "Routes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClimbingRouteModel",
                table: "ClimbingRouteModel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Setter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setter", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_SetterId",
                table: "Routes",
                column: "SetterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_ClimbingRouteModel_ModelId",
                table: "Routes",
                column: "ModelId",
                principalTable: "ClimbingRouteModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Setter_SetterId",
                table: "Routes",
                column: "SetterId",
                principalTable: "Setter",
                principalColumn: "Id");
        }
    }
}
