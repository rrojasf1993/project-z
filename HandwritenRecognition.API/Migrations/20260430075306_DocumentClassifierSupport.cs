using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class DocumentClassifierSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Documents_DocumentId",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Results_OcrResultResultId",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_OcrBoundingBoxes_Lines_SourceLineId",
                table: "OcrBoundingBoxes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lines",
                table: "Lines");

            migrationBuilder.RenameTable(
                name: "Lines",
                newName: "OcrLine");

            migrationBuilder.RenameIndex(
                name: "IX_Lines_OcrResultResultId",
                table: "OcrLine",
                newName: "IX_OcrLine_OcrResultResultId");

            migrationBuilder.RenameIndex(
                name: "IX_Lines_DocumentId",
                table: "OcrLine",
                newName: "IX_OcrLine_DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OcrLine",
                table: "OcrLine",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DocumentTypeExample",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SampleText = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false),
                    EmbeddingVector = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypeExample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTypeExample_DocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypeKeywordRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypeKeywordRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTypeKeywordRules_DocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypeRegexPatternRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegexPattern = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypeRegexPatternRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTypeRegexPatternRules_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypeExample_TypeId",
                table: "DocumentTypeExample",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypeKeywordRules_TypeId",
                table: "DocumentTypeKeywordRules",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypeRegexPatternRules_DocumentTypeId",
                table: "DocumentTypeRegexPatternRules",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OcrBoundingBoxes_OcrLine_SourceLineId",
                table: "OcrBoundingBoxes",
                column: "SourceLineId",
                principalTable: "OcrLine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OcrLine_Documents_DocumentId",
                table: "OcrLine",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OcrLine_Results_OcrResultResultId",
                table: "OcrLine",
                column: "OcrResultResultId",
                principalTable: "Results",
                principalColumn: "ResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OcrBoundingBoxes_OcrLine_SourceLineId",
                table: "OcrBoundingBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_OcrLine_Documents_DocumentId",
                table: "OcrLine");

            migrationBuilder.DropForeignKey(
                name: "FK_OcrLine_Results_OcrResultResultId",
                table: "OcrLine");

            migrationBuilder.DropTable(
                name: "DocumentTypeExample");

            migrationBuilder.DropTable(
                name: "DocumentTypeKeywordRules");

            migrationBuilder.DropTable(
                name: "DocumentTypeRegexPatternRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OcrLine",
                table: "OcrLine");

            migrationBuilder.RenameTable(
                name: "OcrLine",
                newName: "Lines");

            migrationBuilder.RenameIndex(
                name: "IX_OcrLine_OcrResultResultId",
                table: "Lines",
                newName: "IX_Lines_OcrResultResultId");

            migrationBuilder.RenameIndex(
                name: "IX_OcrLine_DocumentId",
                table: "Lines",
                newName: "IX_Lines_DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lines",
                table: "Lines",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Documents_DocumentId",
                table: "Lines",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Results_OcrResultResultId",
                table: "Lines",
                column: "OcrResultResultId",
                principalTable: "Results",
                principalColumn: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_OcrBoundingBoxes_Lines_SourceLineId",
                table: "OcrBoundingBoxes",
                column: "SourceLineId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
