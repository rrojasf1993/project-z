using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2026, 2, 20, 17, 12, 31, 567, DateTimeKind.Local).AddTicks(6359)),
                    ConfidenceAvg = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    RegexPattern = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    ConfidenceWeight = table.Column<float>(type: "real", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                });

            migrationBuilder.CreateTable(
                name: "ImageInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalWidth = table.Column<int>(type: "int", nullable: false),
                    OriginalHeight = table.Column<int>(type: "int", nullable: false),
                    ActualWidth = table.Column<int>(type: "int", nullable: false),
                    ActualHeight = table.Column<int>(type: "int", nullable: false),
                    OcrJobResultId = table.Column<int>(type: "int", nullable: false),
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageInfos_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 60, nullable: false),
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_Jobs_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "ResultId");
                });

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineIndex = table.Column<int>(type: "int", nullable: false),
                    OriginalText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectedText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Confidence = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    OcrResultResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lines_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lines_Results_OcrResultResultId",
                        column: x => x.OcrResultResultId,
                        principalTable: "Results",
                        principalColumn: "ResultId");
                });

            migrationBuilder.CreateTable(
                name: "ProcessDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profile = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProcessingTime = table.Column<int>(type: "int", nullable: false),
                    OcrJobResultId = table.Column<int>(type: "int", nullable: false),
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessDatas_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlurScore = table.Column<float>(type: "real", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    OcrJobResultId = table.Column<int>(type: "int", nullable: false),
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityDatas_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageInfos_ResultId",
                table: "ImageInfos",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ResultId",
                table: "Jobs",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_DocumentId",
                table: "Lines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_OcrResultResultId",
                table: "Lines",
                column: "OcrResultResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDatas_ResultId",
                table: "ProcessDatas",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityDatas_ResultId",
                table: "QualityDatas",
                column: "ResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldRules");

            migrationBuilder.DropTable(
                name: "ImageInfos");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropTable(
                name: "ProcessDatas");

            migrationBuilder.DropTable(
                name: "QualityDatas");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Results");
        }
    }
}
