using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class MissingNavigationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OcrDocumentId",
                table: "ExtractedFields",
                newName: "SourceOcrDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractedFields_SourceOcrDocumentId",
                table: "ExtractedFields",
                column: "SourceOcrDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtractedFields_Documents_SourceOcrDocumentId",
                table: "ExtractedFields",
                column: "SourceOcrDocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtractedFields_Documents_SourceOcrDocumentId",
                table: "ExtractedFields");

            migrationBuilder.DropIndex(
                name: "IX_ExtractedFields_SourceOcrDocumentId",
                table: "ExtractedFields");

            migrationBuilder.RenameColumn(
                name: "SourceOcrDocumentId",
                table: "ExtractedFields",
                newName: "OcrDocumentId");
        }
    }
}
