using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class UserDateToTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "StartDateTimestamp",
                table: "Users",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDateTimestamp",
                table: "Users");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
