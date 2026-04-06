using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class FrontendFriendlyOcrBoundingBoxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OcrBoundingBox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    X = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    Y = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    W = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    H = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    SourceLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcrBoundingBox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OcrBoundingBox_Lines_SourceLineId",
                        column: x => x.SourceLineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OcrBoundingBox_SourceLineId",
                table: "OcrBoundingBox",
                column: "SourceLineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OcrBoundingBox");
        }
    }
}
