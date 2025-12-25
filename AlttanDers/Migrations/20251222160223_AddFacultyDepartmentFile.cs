using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KampusKodu.Migrations
{
    /// <inheritdoc />
    public partial class AddFacultyDepartmentFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Topics");
        }
    }
}
