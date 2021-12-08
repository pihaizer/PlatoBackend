using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    public partial class UpdateRouteTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RouteTags",
                table: "RouteTags");

            migrationBuilder.DropIndex(
                name: "IX_RouteTags_TagId",
                table: "RouteTags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RouteTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RouteTags",
                table: "RouteTags",
                columns: new[] { "TagId", "ClimbingRouteId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RouteTags",
                table: "RouteTags");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "RouteTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RouteTags",
                table: "RouteTags",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RouteTags_TagId",
                table: "RouteTags",
                column: "TagId");
        }
    }
}
