using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class PreprocessImgPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreprocessedFileName",
                table: "Jobs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreprocessedFileName",
                table: "Jobs");
        }
    }
}
