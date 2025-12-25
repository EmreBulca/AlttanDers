using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KampusKodu.Migrations
{
    /// <inheritdoc />
    public partial class AddGamificationAndFilesCorrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Topics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "AspNetUsers");
        }
    }
}
