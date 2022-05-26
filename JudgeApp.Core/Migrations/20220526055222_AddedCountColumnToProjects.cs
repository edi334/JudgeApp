using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JudgeApp.Core.Migrations
{
    public partial class AddedCountColumnToProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Projects");
        }
    }
}
